using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace backend_aspnetcore.Services
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Sauvegarde un fichier upload� dans le syst�me de stockage
        /// </summary>
        /// <param name="file">Fichier � sauvegarder</param>
        /// <returns>Chemin complet du fichier sauvegard�</returns>
        Task<string> SaveFileAsync(IFormFile file);

        /// <summary>
        /// Supprime un fichier du syst�me de stockage
        /// </summary>
        /// <param name="filePath">Chemin complet du fichier � supprimer</param>
        void DeleteFile(string filePath);
    }
}