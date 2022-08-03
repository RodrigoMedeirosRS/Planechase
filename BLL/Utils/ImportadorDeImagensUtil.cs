using System;
using Godot;

namespace BLL.Utils
{
    internal static class ImportadorDeImagensUtil
    {
        internal static long Counter { get; set; } = 0;
        internal static Texture BuscarImagem(string nomeImagem, string caminho)
        {
            var caminhoComFormato = caminho + nomeImagem + ".tres";
            return GD.Load<StreamTexture>(caminhoComFormato);
        }
    }
}