using System;
using Godot;

namespace BLL.Utils
{
    internal static class ImportadorDeImagensUtil
    {
        internal static long Counter { get; set; } = 0;
        internal static Texture BuscarImagem(string nomeImagem, string caminho)
        {
            var imagem = new Image();
            var texturaDaImagem = new ImageTexture();
            var caminhoComFormato = caminho + nomeImagem + ".jpg";
            var caminhoImport = caminhoComFormato + ".import";

            imagem.Load(caminhoComFormato);
            texturaDaImagem.CreateFromImage(imagem);
            LimparArquivosTemporarios(caminhoComFormato, caminhoImport);
            
            return texturaDaImagem;
        }
        private static void LimparArquivosTemporarios(string caminhoComFormato, string caminhoImport)
        {
            if (System.IO.File.Exists(caminhoComFormato))
                System.IO.File.Delete(caminhoComFormato);
            if (System.IO.File.Exists(caminhoImport))
                System.IO.File.Delete(caminhoImport);
        }
    }
}