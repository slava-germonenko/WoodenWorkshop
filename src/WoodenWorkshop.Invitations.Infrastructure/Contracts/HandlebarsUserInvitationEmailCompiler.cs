using System.Text;

using Azure.Storage.Blobs;
using HandlebarsDotNet;

using WoodenWorkshop.Invitations.Core.Contracts;

namespace WoodenWorkshop.Invitations.Infrastructure.Contracts;

public class HandlebarsUserInvitationEmailCompiler : IUserInvitationEmailCompiler
{
    private const string EmailTemplatesContainerName = "email-templates";

    private const string UserInvitationFileName = "user-invitation.hbs";

    private readonly BlobServiceClient _blobServiceClient;

    private BlobContainerClient BlobServiceClient =>
        _blobServiceClient.GetBlobContainerClient(EmailTemplatesContainerName);

    public HandlebarsUserInvitationEmailCompiler(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> CompileUserInvitationAsync()
    {
        var downloadResult = await BlobServiceClient.GetBlobClient(UserInvitationFileName).DownloadContentAsync();
        var emailTemplate = downloadResult.Value.Content.ToString();
        return Handlebars.Compile(emailTemplate)(new object());
    }
}