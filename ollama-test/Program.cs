using System;
using System.Threading.Tasks;
using OllamaTest;

class Program
{
    static async Task Main(string[] args)
    {
        var ollamaClient = new OllamaClient(model: "llama3");

        Console.WriteLine("Hafızalı Local AI Bot'a Hoş Geldiniz!");
        Console.WriteLine("(Çıkış için 'çıkış' yazın)\n");

        while (true)
        {
            Console.Write("Sen: ");
            string kullaniciSorusu = Console.ReadLine();

            if (kullaniciSorusu.ToLower() == "çıkış") break;
            if (string.IsNullOrWhiteSpace(kullaniciSorusu)) continue;

            Console.WriteLine("AI Düşünüyor...");

            try
            {
                string aiResponse = await ollamaClient.SendMessageAsync(kullaniciSorusu);
                Console.WriteLine("llama3: " + aiResponse + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message + "\n");
            }
        }
    }
}