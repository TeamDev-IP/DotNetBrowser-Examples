namespace ExcelComAddin.Interop
{
    /// <summary>
    /// COM identity constants shared between <see cref="Connect"/> and <see cref="ComRegistration"/>.
    /// </summary>
    public static class ComConstants
    {
        /// <summary>The ProgId used to register and locate the add-in COM class.</summary>
        public const string ProgId = "ExcelComAddin.Connect";

        /// <summary>The stable GUID that identifies the <see cref="Connect"/> COM class.</summary>
        public const string RootClassGuid = "A1B2C3D4-E5F6-7890-ABCD-EF1234567880";
    }
}
