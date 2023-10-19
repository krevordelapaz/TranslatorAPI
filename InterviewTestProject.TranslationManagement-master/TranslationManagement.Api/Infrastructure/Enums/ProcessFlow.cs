using System.ComponentModel;

namespace TranslationManagement.Api.Infrastructure.Enums
{
    public enum ProcessFlow
    {
        [Description("Creation successful")]
        Created,
        [Description("Creation failed")]
        CreationFailed,
        [Description("Updating successful")]
        Updated,
        [Description("Updating failed")]
        UpdatingFailed,
        [Description("Deletion successful")]
        Deleted,
        [Description("Deletion failed")]
        DeletionFailed
    }
}
