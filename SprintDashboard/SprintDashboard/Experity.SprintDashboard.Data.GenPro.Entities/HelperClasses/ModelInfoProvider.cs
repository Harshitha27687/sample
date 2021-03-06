﻿//////////////////////////////////////////////////////////////
// <auto-generated>This code was generated by LLBLGen Pro v5.5.</auto-generated>
//////////////////////////////////////////////////////////////
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
//////////////////////////////////////////////////////////////

using Experity.SprintDashboard.Data.GenPro.Entities.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Experity.SprintDashboard.Data.GenPro.Entities.HelperClasses
{
    /// <summary>Singleton implementation of the ModelInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.</summary>
    public static class ModelInfoProviderSingleton
    {
        private static readonly IModelInfoProvider _providerInstance = new ModelInfoProviderCore();

        /// <summary>Dummy static constructor to make sure threadsafe initialization is performed.</summary>
        static ModelInfoProviderSingleton() { }

        /// <summary>Gets the singleton instance of the ModelInfoProviderCore</summary>
        /// <returns>Instance of the FieldInfoProvider.</returns>
        public static IModelInfoProvider GetInstance()
        {
            return _providerInstance;
        }
    }

    /// <summary>Actual implementation of the ModelInfoProvider.</summary>
    internal class ModelInfoProviderCore : ModelInfoProviderBase
    {
        /// <summary>Initializes a new instance of the <see cref="ModelInfoProviderCore"/> class.</summary>
        internal ModelInfoProviderCore()
        {
            Init();
        }

        /// <summary>Method which initializes the internal datastores.</summary>
        private void Init()
        {
            this.InitClass();
            InitClinicEntityInfo();
            InitPracticeEntityInfo();
            this.BuildInternalStructures();
        }

        /// <summary>Inits ClinicEntity's info objects</summary>
        private void InitClinicEntityInfo()
        {
            this.AddFieldIndexEnumForElementName(typeof(ClinicFieldIndex), "ClinicEntity");
            this.AddElementFieldInfo("ClinicEntity", "ClinicPk", typeof(System.Guid), true, false, false, false, (int)ClinicFieldIndex.ClinicPk, 0, 0, 0);
            this.AddElementFieldInfo("ClinicEntity", "Name", typeof(System.String), false, false, false, false, (int)ClinicFieldIndex.Name, 10, 0, 0);
            this.AddElementFieldInfo("ClinicEntity", "PracticePk", typeof(System.Guid), false, true, false, false, (int)ClinicFieldIndex.PracticePk, 0, 0, 0);
            this.AddElementFieldInfo("ClinicEntity", "UtcOffset", typeof(System.Int16), false, false, false, false, (int)ClinicFieldIndex.UtcOffset, 0, 0, 5);
        }

        /// <summary>Inits PracticeEntity's info objects</summary>
        private void InitPracticeEntityInfo()
        {
            this.AddFieldIndexEnumForElementName(typeof(PracticeFieldIndex), "PracticeEntity");
            this.AddElementFieldInfo("PracticeEntity", "Environment", typeof(System.String), false, false, false, false, (int)PracticeFieldIndex.Environment, 10, 0, 0);
            this.AddElementFieldInfo("PracticeEntity", "Practice", typeof(System.String), false, false, false, false, (int)PracticeFieldIndex.Practice, 5, 0, 0);
            this.AddElementFieldInfo("PracticeEntity", "PracticePk", typeof(System.Guid), true, false, false, false, (int)PracticeFieldIndex.PracticePk, 0, 0, 0);
        }
    }
}