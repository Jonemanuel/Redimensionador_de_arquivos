using System.Drawing;
using System;
using System.IO;
using System.Threading;


namespace didaticos.redimensionador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando redimensionador");
            Thread thread = new Thread(Redimensionador);
            thread.Start();

        }

        static void Redimensionador()
        {
            #region "Diretorios"
            string diretorio_entrada = "Arquivo_Entrada";
            string diretorio_redimensionar = "Arquivo_Redimensionado";
            string diretorio_finalizados = "Arquivo_Finalizados";

            if (!Directory.Exists(diretorio_entrada))

            {
                Directory.CreateDirectory(diretorio_entrada);
            }

            if (!Directory.Exists(diretorio_redimensionar))


            {
                Directory.CreateDirectory(diretorio_redimensionar);
            }
            if (!Directory.Exists(diretorio_finalizados))


            {
                Directory.CreateDirectory(diretorio_finalizados);
            }
            #endregion
            FileStream fileStream;
            FileInfo fileInfo;
            while (true) 
            {
                //Meu programa vai olhar para a pasta de entrada
                //Se tiver arquivo, ele irá redimensionar
                var arquivosEntrada = Directory.EnumerateFiles(diretorio_entrada);

                int novaAltura = 200;

                foreach (var arquivo in arquivosEntrada) 
                {
                    fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fileInfo = new FileInfo(arquivo);

                    String caminho = Environment.CurrentDirectory + @"\" + diretorio_redimensionar
                        + @"\" + DateTime.Now.Millisecond.ToString() + "_" + fileInfo.Name ;

                    Redimensionador (Image.FromStream(fileStream), novaAltura, caminho);

                    fileStream.Close();

                    String caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_finalizados + @"\" + fileInfo.Name;

                    fileInfo.MoveTo(caminhoFinalizado);
                }

                Thread.Sleep(new TimeSpan(0, 0, 5));
            }
        }

        

        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImage = new Bitmap(novaLargura, novaAltura);

            using (Graphics g = Graphics.FromImage(novaImage))
            {
                g.DrawImage (imagem, 0, 0, novaLargura, novaAltura);

            }
            novaImage.Save(caminho);
            imagem.Dispose();
        }
    }
}
