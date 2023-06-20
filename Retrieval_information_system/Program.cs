using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;



namespace Retrieval_information_system
{
    class Program
    {
        static void Main(string[] args)
        {
            // Menentukan dokumen atau informasi yang akan disimpan
            List<string> documents = new List<string>
            {
                
            };

            // Meminta pengguna memasukkan jumlah dokumen yang ingin dibandingkan
            Console.WriteLine("Masukkan jumlah dokumen yang ingin Anda bandingkan:");
            int numDocuments = int.Parse(Console.ReadLine());

            // Menghitung vektor fitur untuk setiap dokumen yang ingin dibandingkan
            List<Dictionary<string, int>> documentVectors = new List<Dictionary<string, int>>();
            for (int i = 0; i < numDocuments; i++)
            {
                Console.WriteLine("\nMasukkan teks untuk dokumen " + (i + 1) + ":");
                string document = Console.ReadLine();
                Dictionary<string, int> vector = CalculateFeatureVector(document);
                documentVectors.Add(vector);
            }

            // Meminta pengguna memasukkan query atau teks
            Console.WriteLine("\nMasukkan query atau teks:");
            string query = Console.ReadLine();

           
            // Menghilangkan stop word dari dokumen dan query
            List<string> stopWords = GetStopWords(); // Mengambil stop word list
            documents = RemoveStopWords(documents, stopWords);
            query = RemoveStopWords(query, stopWords);

            // Menghitung vektor fitur untuk query
            Dictionary<string, int> queryVector = CalculateFeatureVector(query);

            // Menghitung Cosine Similarity antara query dan setiap dokumen
            List<double> similarities = new List<double>();
            foreach (Dictionary<string, int> documentVector in documentVectors)
            {
                double similarity = CalculateCosineSimilarity(queryVector, documentVector);
                similarities.Add(similarity);
            }

            // Menampilkan hasil Cosine Similarity kepada pengguna
            for (int i = 0; i < similarities.Count; i++)
            {
                Console.WriteLine("\nCosine Similarity dengan dokumen " + (i + 1) + ": " + similarities[i]);
            }

            
            Console.WriteLine("\nPress any key to close...");
            Console.ReadKey();
        }

        // Fungsi untuk menghilangkan stop word dari teks
        static string RemoveStopWords(string text, List<string> stopWords)
        {
            string[] words = text.Split(' ');
            List<string> filteredWords = words.Where(word => !stopWords.Contains(word)).ToList();
            string filteredText = string.Join(" ", filteredWords);
            return filteredText;
        }

        // Fungsi untuk menghilangkan stop word dari setiap dokumen dalam daftar
        static List<string> RemoveStopWords(List<string> documents, List<string> stopWords)
        {
            List<string> filteredDocuments = new List<string>();
            foreach (string document in documents)
            {
                string filteredDocument = RemoveStopWords(document, stopWords);
                filteredDocuments.Add(filteredDocument);
            }
            return filteredDocuments;
        }

        // Fungsi untuk menghitung vektor fitur dari sebuah dokumen
        static Dictionary<string, int> CalculateFeatureVector(string document)
        {
            Dictionary<string, int> vector = new Dictionary<string, int>();

            // Memisahkan kata-kata dalam dokumen
            string[] words = document.Split(' ');

            // Menghitung frekuensi kemunculan kata dalam dokumen
            foreach (string word in words)
            {
                if (vector.ContainsKey(word))
                {
                    vector[word]++;
                }
                else
                {
                    vector[word] = 1;
                }
            }

            return vector;
        }

        // Fungsi untuk menghitung Cosine Similarity antara dua vektor
        static double CalculateCosineSimilarity(Dictionary<string, int> vector1, Dictionary<string, int> vector2)
        {
            double dotProduct = 0;
            double magnitude1 = 0;
            double magnitude2 = 0;

            // Menghitung dot product dan magnitudo vektor
            foreach (string key in vector1.Keys)
            {
                if (vector2.ContainsKey(key))
                {
                    dotProduct += vector1[key] * vector2[key];
                }
                magnitude1 += Math.Pow(vector1[key], 2);
            }

            foreach (string key in vector2.Keys)
            {
                magnitude2 += Math.Pow(vector2[key], 2);
            }

            magnitude1 = Math.Sqrt(magnitude1);
            magnitude2 = Math.Sqrt(magnitude2);

            // Menghitung Cosine Similarity
            double similarity = dotProduct / (magnitude1 * magnitude2);
            return similarity;
        }

        // Fungsi untuk mendapatkan stop word list bahasa Indonesia
        static List<string> GetStopWords()
        {
            List<string> stopWords = new List<string>()
            {
                "dan", "atau", "juga", "adalah", "dalam", "pada", "di", "ke", "yang", "ini", "itu"               
            };
            return stopWords;
        }

    }
}
