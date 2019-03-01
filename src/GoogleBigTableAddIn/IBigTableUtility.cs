using Scripting;
using System.Runtime.InteropServices;

namespace GoogleBigTableAddIn
{
    [ComVisible(true)]
    public interface IBigTableUtilityCOM
    {
        Dictionary GetAllRecords(string projectId, string instanceId, string tableName, string credentialPath, bool isFilterApplied, string rowCount);
    }
}
