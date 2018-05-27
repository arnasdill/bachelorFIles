using PatternRecognition.FingerprintRecognition.Core;
using System;
using PatternRecognition.FingerprintRecognition.FeatureExtractors;
using PatternRecognition.FingerprintRecognition.Matchers;
using PatternRecognition.FingerprintRecognition.FeatureRepresentation;
using System.IO;


namespace PatternRecognition.FingerprintRecognition
{/*


TOOOOOOOOOOOOOOOOO DOOOOOOOOOOOOOOOO

    issiaksinti kaip ten tas eer skaiciuojasi, 
    pasidaryti testa main programoj is naujo si JY ir su taips paciais extractoriais
    viska padaryt be hardcodo, o su parametrais





    */
    class ImageInfo
    {
        public int firstDigit;
        public int secondDigit;
    }
    class sortedFmrList
    {
        public int firstDigit1;
        public int secondDigit1;
        public int firstDigit2;
        public int secondDigit2;
        public double score;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller();
            controller.Start();
           /* sortedFmrList[] fmrList = new sortedFmrList[500];
            int p = 0;
            ImageInfo[] possibleImages = new ImageInfo[81];
            for(int i = 1; i < 11; i++)
            {
                for(int j = 1; j< 9; j++)
                {
                    possibleImages[p] = new ImageInfo();
                    possibleImages[p].firstDigit = 100 + i;
                    possibleImages[p].secondDigit = j;
                    p++;
                }
               
            }*/
            /*var featExtractor = new JYFeatureExtractor()
             {
                 MtiaExtractor = new Ratha1995MinutiaeExtractor(),
             SkeletonImgExtractor = new Ratha1995SkeImgExtractor()
             };*/
            //var matcher = new JY();
            //matcher.GlobalDistThr = 1;
            /* var watch = System.Diagnostics.Stopwatch.StartNew();
             var fingerprintImg1 = ImageLoader.LoadImage("C:/Users/Arnas/Desktop/kursinis/Databases/2002/DB1_B/101_1.tif");
             var features1 = featExtractor.ExtractFeatures(fingerprintImg1);
             JYFeatures[] features = new JYFeatures[9];
              features [0] = features1;
              for ( int i =1; i< 9; i++)
              {

                  var fingerprintImg2 = ImageLoader.LoadImage("C:/Users/Arnas/Desktop/kursinis/Databases/2002/DB1_B/102_" + i +".tif");


                  var features2 = featExtractor.ExtractFeatures(fingerprintImg2);
                  features[i] = features2;
                  double similarity = matcher.Match(features1, features2);
                  Console.WriteLine(similarity.ToString());
              }
              watch.Stop();
              var elapsedMs = watch.ElapsedMilliseconds / 1000;
              Console.WriteLine(elapsedMs);
              string FullPath = "C:/Users/Arnas/Desktop/temp/rez.jy";
             if (!File.Exists(FullPath)){
                 Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
                 BinarySerializer.Serialize(features, FullPath);
             }*/
           /* double totalMatches = 0;
            double fnmrCount = 0;
            int fmrCount = 0;
            matcher.GlobalAngleThr = 29.99999999999999;
            matcher.GlobalDistThr = 8;
            if (File.Exists("C:/Users/Arnas/Desktop/temp/2002/DB1_B/rez.jy"))
            {
                JYFeatures[] features = new JYFeatures[81];
                features = (FingerprintRecognition.FeatureRepresentation.JYFeatures[])BinarySerializer.Deserialize("C:/Users/Arnas/Desktop/temp/2002/DB1_B/rez.jy");
                
                int trueMatch = 0;
                int falseMatch = 0;
                double matchResult;
                for (int j = 0; j< 80; j++)
             {
                    for(int i = j; i < 80; i++)
                    {

                        if (possibleImages[i].secondDigit == possibleImages[j].secondDigit || (possibleImages[i].firstDigit == possibleImages[j].firstDigit && possibleImages[i].secondDigit != possibleImages[j].secondDigit))

                        {
                            if (possibleImages[i].firstDigit == possibleImages[j].firstDigit && possibleImages[i].secondDigit != possibleImages[j].secondDigit)
                            {
                                matchResult = matcher.Match(features[i], features[j]);
                                if (matchResult <= 16.9)
                                {
                                    fnmrCount++;
                                }

                                Console.WriteLine("false not match in image" + possibleImages[i]);

                            }
                            if (possibleImages[i].firstDigit != possibleImages[j].firstDigit)
                            {
                                matchResult = matcher.Match(features[i], features[j]);
                                if (matchResult > 16.9)
                                {
                                    fmrList[fmrCount] = new sortedFmrList();
                                    fmrList[fmrCount].firstDigit1 = possibleImages[j].firstDigit;
                                    fmrList[fmrCount].secondDigit1 = possibleImages[j].secondDigit;
                                    fmrList[fmrCount].firstDigit2 = possibleImages[i].firstDigit;
                                    fmrList[fmrCount].secondDigit2 = possibleImages[i].secondDigit;
                                    fmrList[fmrCount].score = matchResult;
                                    fmrCount++;
                                }

                            }
                            totalMatches++;
                        } 

                    }
                 
             }
            }*/
           // generateFeatures();
           /*for(int i =0; i < fmrCount; i++)
            {
                Console.WriteLine("false match image" + fmrList[i].firstDigit1 + "_" + fmrList[i].secondDigit1
                  + " with " + fmrList[i].firstDigit1 + "_" + fmrList[i].secondDigit2 + ". Score: "+ fmrList[i].score);
            }
            Console.WriteLine(totalMatches + " " + fmrCount + " " + fnmrCount);
            Console.WriteLine("EER:" +fmrCount + " " + fnmrCount);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();*/

        }

    
    public static  void generateFeatures()
        {
            int z = 0;
            JYFeatures[] features = new JYFeatures[81];
            var featExtractor = new DelauneyMTpsExtractor()
            {
                MtiaExtractor = new Ratha1995MinutiaeExtractor(),
                SkeletonImgExtractor = new Ratha1995SkeImgExtractor()

            };
            for (int j = 1; j < 11; j++)
            {

            for (int i = 1; i < 9; i++)
                {
                   
                    if ( j < 10)
                    {
                        var fingerprintImg = ImageLoader.LoadImage("C:/Users/Arnas/Desktop/kursinis/Databases/2002/DB1_B/10" + j + "_" + i + ".tif");
                        features[z] = featExtractor.ExtractFeatures(fingerprintImg);
                    }
                    else
                    {
                       var  fingerprintImg = ImageLoader.LoadImage("C:/Users/Arnas/Desktop/kursinis/Databases/2002/DB1_B/1" + j + "_" + i + ".tif");
                        features[z] = featExtractor.ExtractFeatures(fingerprintImg);
                    }
                    
                    Console.WriteLine("1"+j+"_"+i+".tif");
                    z++;
                }
            }
            string FullPath = "C:/Users/Arnas/Desktop/temp/2002/DB1_B/rez.jy";

                Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
                BinarySerializer.Serialize(features, FullPath);
            
        }
    }
}
