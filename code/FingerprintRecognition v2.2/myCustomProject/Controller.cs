using PatternRecognition.FingerprintRecognition.Core;
using PatternRecognition.FingerprintRecognition.FeatureExtractors;
using PatternRecognition.FingerprintRecognition.FeatureRepresentation;
using PatternRecognition.FingerprintRecognition.Matchers;
using System;

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PatternRecognition.FingerprintRecognition
{
    class Controller
    {

   
        class MatchList
        {
            public int firstDigit1;
            public int secondDigit1;
            public int firstDigit2;
            public int secondDigit2;
            public double score;
        }

        public class ImageInfo
        {
            public int firstDigit;
            public int secondDigit;
        }
        /************PUBLIC VARIABLES****************/
        public ImageInfo[] possibleImages = new ImageInfo[81];
        private Tico2003Features[] features = new Tico2003Features[81];
        const int defaultImgCount = 80;
        MatchList[] fmrList = new MatchList[10000];
        MatchList[] fnmrList = new MatchList[10000];
        TK matcherJY = new TK();
       
        int fmrCount = 0;
        int fnmrCount = 0;
        double totalMatchesFmr = 0;
        double totalMatchesFnmr = 0;



        public double treshold = 12;
        public double globalAngleThr = 24;
        public double globalDistThr = 0;
        string algExtenstion = "TK";
        string currentDB = "2002/DB4_B";


        double tresholdStart = 0.1;
        double globalAngleThrStart = 10;
        double globalDistThrStart = 4;

        /********************************************/

        public void intializeImagesNames()
        {
            int p = 0;
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    possibleImages[p] = new ImageInfo();
                    possibleImages[p].firstDigit = 100 + i;
                    possibleImages[p].secondDigit = j;
                    p++;
                }

            }
        }

        public void Start()
        {
            intializeImagesNames();
            matcherJY.GlobalAngleThr = 29.99999999999999;
            matcherJY.GlobalDistThr = 8;
            if (File.Exists("C:/Users/Arnas/Desktop/temp/"+ currentDB + "/" + algExtenstion + "/features."+algExtenstion))
            {
               
                features = (Tico2003Features[])BinarySerializer.Deserialize("C:/Users/Arnas/Desktop/temp/" + currentDB + "/" + algExtenstion + "/features." + algExtenstion);
            }

            generate3params(currentDB);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }


        public double GetFMR(Tico2003Features[] featuresList)
        {
            double fmr = 0;
            double tempScore;
            for (int i = 0; i < defaultImgCount; i++)
            {
                for(int j = i; j< defaultImgCount; j++)
                {
                    if(possibleImages[i].firstDigit != possibleImages[j].firstDigit && possibleImages[i].secondDigit == possibleImages[j].secondDigit)
                    { // remove text after && in if to do matching each to rest.
                        tempScore = matcherJY.Match(features[i], features[j]);
                        
                        if (tempScore >= treshold)
                        {
                           // Console.WriteLine(i + " " + j);
                            fmrList[fmrCount] = new MatchList();
                            fmrList[fmrCount].firstDigit1 = possibleImages[i].firstDigit;
                            fmrList[fmrCount].secondDigit1 = possibleImages[i].secondDigit;
                            fmrList[fmrCount].firstDigit2 = possibleImages[j].firstDigit;
                            fmrList[fmrCount].secondDigit2 = possibleImages[j].secondDigit;
                            fmrList[fmrCount].score = tempScore;

                            fmrCount++;
                        }
                        totalMatchesFmr++;
                    }
                }
            }
            /*for(int i =0; i < fmrCount; i++)
            {
                Console.WriteLine("false match image" + fmrList[i].firstDigit1 + "_" + fmrList[i].secondDigit1
                  + " with " + fmrList[i].firstDigit2 + "_" + fmrList[i].secondDigit2 + ". Score: "+ fmrList[i].score);
            }*/
            fmr = fmrCount/ totalMatchesFmr;
            //Console.WriteLine("Total matches:" + totalMatches);
            //Console.WriteLine("fmrCount:" + fmrCount);
            //Console.WriteLine("FMR percentage:" + fmr);
           
            return fmr;
        }
        public string MatchingResult()
        {
            return "Total matches: " + (totalMatchesFmr + totalMatchesFnmr) + ". FMR: " + fmrCount / totalMatchesFmr + ", FNMR: " + fnmrCount / totalMatchesFnmr
                + ". EER: " + 100 * (fmrCount / totalMatchesFmr + fnmrCount / totalMatchesFnmr) / 2;
        }
        public void generateTresholds(string path)
        {
            
            int generateCount = 90;
            using (var w = new StreamWriter(@"C:\Users\Arnas\Desktop\temp\" + path + @"\threshold"+ algExtenstion +".csv"))
            {
                w.WriteLine("TRESHOLD" + " " + "EER" + " " + "FMR" + " " + "FNMR");
                w.Flush();
                for (int i = 0; i < generateCount; i += 3)
                {
                    Console.WriteLine("Loading... Current percentage: " + Math.Round((double)i / generateCount * 100, 1) + "%  (can finish faster)");
                    treshold = i;
                    double tempFnmr = GetFNMR(features);
                    double tempFmr = GetFMR(features);
                    if(tempFmr == 0)
                    {
                        break;
                    }
                    double tempValue = Math.Round((100 * (tempFnmr + tempFmr)) / 2, 4);
                    w.WriteLine(treshold + " "+ tempValue + " " + tempFmr + " " + tempFnmr);
                    w.Flush();
                    // sw.WriteLine(tempValue + "," + treshold);
                    //sw.WriteLine(line);  +  +  + );
                    Console.Clear();
                    totalMatchesFmr = 0;
                    totalMatchesFnmr = 0;
                    fmrCount = 0;
                    fnmrCount = 0;
                }
                w.Flush();
            }
    
            
        }
        public void generateGlobalAngleThr(string path)
        {

            int generateCount = 90;
            using (var w = new StreamWriter(@"C:\Users\Arnas\Desktop\temp\" + path + @"\globalAngleThr" + algExtenstion + ".csv"))
            {
                w.WriteLine("GATHR" + " " + "EER" + " " + "FMR" + " " + "FNMR");
                w.Flush();
                w.WriteLine("TRESHOLD" + " " + treshold);
                w.Flush();
                for (int i = 0; i < generateCount; i += 3)
                {
                    Console.WriteLine("Loading... Current percentage: " + Math.Round((double)i / generateCount * 100, 1) + "%  (can finish faster)");
                    matcherJY.GlobalAngleThr = i;
                    double tempFnmr = GetFNMR(features);
                    double tempFmr = GetFMR(features);
                    double tempValue = Math.Round((100 * (tempFnmr + tempFmr)) / 2, 4);
                    w.WriteLine(matcherJY.GlobalAngleThr + " " + tempValue + " " + tempFmr + " " + tempFnmr);
                    w.Flush();
                    // sw.WriteLine(tempValue + "," + treshold);
                    //sw.WriteLine(line);  +  +  + );
                    Console.Clear();
                    totalMatchesFmr = 0;
                    totalMatchesFnmr = 0;
                    fmrCount = 0;
                    fnmrCount = 0;
                }
                w.Flush();
            }


        }
        public void generate3params(string path)
        {
            double matchesInTest = 0;
            int generateCount = 20;
            using (var w = new StreamWriter(@"C:\Users\Arnas\Desktop\temp\" + path + "/" + algExtenstion + @"\3params" + algExtenstion + ".csv"))
            {
                w.WriteLine("EER" + " " + "FMR" + " " + "FNMR" +  " " + "THR" + " " + "GDTHR" + " "+ "GAngTHR"+" " + "MtiaCTHR" );
                w.Flush();

                var watch = System.Diagnostics.Stopwatch.StartNew();
                for (double i = 0.004; i <= 0.016; i+= 0.004)
                {
                    treshold = i;
                    for(double k = 10; k <= 60; k += 10)
                    {
                        globalAngleThr = k;
                        for(double z = 4; z <= 10; z += 2)
                        {
                            globalDistThr = z;
                            for (double q = 2; q <= 10; q+= 2)
                            {
                                matcherJY.MtiaCountThr = (int)q;
                                        Console.Write(i + " " + k + " " + z + " " + q);

                                        matcherJY.GlobalDistThr = (int)globalDistThr;

                                        matcherJY.GlobalAngleThr = globalAngleThr;
                                        double tempFnmr = GetFNMR(features);
                                        double tempFmr = GetFMR(features);
                                        double tempValue = Math.Round((100 * (tempFnmr + tempFmr)) / 2, 4);
                                        w.WriteLine(tempValue + " " + tempFmr + " " + tempFnmr + " " + treshold + " " + matcherJY.GlobalDistThr + " " + matcherJY.GlobalAngleThr
                                            +" " + matcherJY.MtiaCountThr);
                                        w.Flush();
                                        Console.Clear();
                                        matchesInTest += totalMatchesFmr + totalMatchesFnmr;
                                        totalMatchesFmr = 0;
                                        totalMatchesFnmr = 0;
                                        fmrCount = 0;
                                        fnmrCount = 0;

                                    
                                

                            }
                           
                        }
                    }

                }
                w.WriteLine("Matches total" + " " + matchesInTest);
                w.Flush();
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds / 1000;
                w.WriteLine(elapsedMs);
                w.Flush();

            }


        }
        public void generateGlobalDistThr(string path)
        {
            double matchesInTest = 0;
            int generateCount = 20;
            using (var w = new StreamWriter(@"C:\Users\Arnas\Desktop\temp\" + path + "/" + algExtenstion  + @"\globalDistThr" + algExtenstion + ".csv"))
            {
                w.WriteLine("GDTHR" + " " + "EER" + " " + "FMR" + " " + "FNMR");
                w.Flush();
                w.WriteLine("TRESHOLD" + " " + treshold + " " + "GlobAngThr" + " " + globalAngleThr);
                w.Flush();
                for (int i = 0; i < generateCount; i += 1)
                {
                    Console.WriteLine("Loading... Current percentage: " + Math.Round((double)i / generateCount * 100, 1) + "%  (can finish faster)");
                    matcherJY.GlobalDistThr = i;
                    double tempFnmr = GetFNMR(features);
                    double tempFmr = GetFMR(features);
                    double tempValue = Math.Round((100 * (tempFnmr + tempFmr)) / 2, 4);
                    w.WriteLine(matcherJY.GlobalDistThr + " " + tempValue + " " + tempFmr + " " + tempFnmr);
                    w.Flush();
                    Console.Clear();
                    matchesInTest += totalMatchesFmr + totalMatchesFnmr;
                    totalMatchesFmr = 0;
                    totalMatchesFnmr = 0;
                    fmrCount = 0;
                    fnmrCount = 0;
                }
                w.WriteLine("Matches total"  + " " + matchesInTest);
                w.Flush();
   
            }


        }
        public void GetScore(string algorithm, string databasePath)
        {
            string fullPath = "C:/Users/Arnas/Desktop/temp/" + databasePath;
            switch (algorithm)
            {

            }

        }
        public double GetFNMR(Tico2003Features[] featuresList)
        {
            
            double fnmr = 0;
            double score;
            for (int i = 0; i < defaultImgCount; i++)
            {
                for (int j = i; j < defaultImgCount; j++)
                {
                    if (possibleImages[i].firstDigit == possibleImages[j].firstDigit)
                    {
                        score = matcherJY.Match(features[i], features[j]);

                        if (score < treshold)
                        {
                            //Console.WriteLine(i + " " + j);
                            fmrList[fnmrCount] = new MatchList();
                            fmrList[fnmrCount].firstDigit1 = possibleImages[j].firstDigit;
                            fmrList[fnmrCount].secondDigit1 = possibleImages[j].secondDigit;
                            fmrList[fnmrCount].firstDigit2 = possibleImages[i].firstDigit;
                            fmrList[fnmrCount].secondDigit2 = possibleImages[i].secondDigit;
                            fmrList[fnmrCount].score = score;
                            fnmrCount++;
                        }
                        totalMatchesFnmr++;
                    }
                }
            }
            fnmr = fnmrCount / totalMatchesFnmr;
            //Console.WriteLine("Total matches:" + totalMatches);
            //Console.WriteLine("fnmrCount:" + fnmrCount);
            //Console.WriteLine("FNMR percentage:" + fnmr);
            return fnmr;
        }

        public void generateFeatures()
        {
            
          /*  int z = 0;
            var featExtractor = new DelauneyMTpsExtractor()
            {
                MtiaExtractor = new Ratha1995MinutiaeExtractor()
            };
            for (int j = 1; j < 11; j++)
            {

                for (int i = 1; i < 9; i++)
                {

                    if (j < 10)
                    {
                        var fingerprintImg = ImageLoader.LoadImage("C:/Users/Arnas/Desktop/kursinis/Databases/" + currentDB + "/10" + j + "_" + i + ".tif");
                        features[z] = featExtractor.ExtractFeatures(fingerprintImg);
                    }
                    else
                    {
                        var fingerprintImg = ImageLoader.LoadImage("C:/Users/Arnas/Desktop/kursinis/Databases/" + currentDB + "/1" + j + "_" + i + ".tif");
                        features[z] = featExtractor.ExtractFeatures(fingerprintImg);
                    }

                    Console.WriteLine("1" + j + "_" + i + ".tif");
                    z++;
                }
            }
            string FullPath = "C:/Users/Arnas/Desktop/temp/"+currentDB +"/" + algExtenstion+"/features." + algExtenstion;

            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
            BinarySerializer.Serialize(features, FullPath);
*/
        }

    }
}
