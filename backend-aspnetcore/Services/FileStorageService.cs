using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace backend_aspnetcore.Services
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Sauvegarde un fichier uploadé dans le système de stockage
        /// </summary>
        /// <param name="file">Fichier à sauvegarder</param>
        /// <returns>Chemin complet du fichier sauvegardé</returns>
        Task<string> SaveFileAsync(IFormFile file);

        /// <summary>
        /// Supprime un fichier du système de stockage
        /// </summary>
        /// <param name="filePath">Chemin complet du fichier à supprimer</param>
        void DeleteFile(string filePath);
    }
}