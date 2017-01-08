using System.Threading.Tasks;
using Xemio.Client.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Apps.Windows.Services.Commands.Folders
{
    public class CreateFolderCommand : ICommand
    {
        public string Name { get; }
        public long? ParentFolderId { get; }

        public CreateFolderCommand(string name, long? parentFolderId = null)
        {
            this.Name = name;
            this.ParentFolderId = parentFolderId;
        }
    }

    public class CreateFolderCommandHandler : ICommandHandler<CreateFolderCommand>
    {
        private readonly IFoldersClient _foldersClient;

        public CreateFolderCommandHandler(IFoldersClient foldersClient)
        {
            this._foldersClient = foldersClient;
        }

        public async Task ExecuteAsync(CreateFolderCommand command)
        {
            var createFolder = new CreateFolder
            {
                Name = command.Name,
                ParentFolderId = command.ParentFolderId
            };

            FolderDTO folder =  await this._foldersClient.CreateFolderAsync(createFolder);
        }
    }
}