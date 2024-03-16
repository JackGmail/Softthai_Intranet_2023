namespace Backend.Enum
{
    public class EnumEmployee
    {
        #region Enum
        public enum DataTypeID
        {
            nTypeEducation = 15,
            nTypeLang = 16,
            nTypeTask = 17,
            nImageType = 88,
            nProgramskills = 91,
            nProgramlanguageskills = 92,
            nTypeSpecial = 18,
            nMilitaryStatus = 8,
            nStatus = 9,
            nSex = 10,
            nHousingType = 12,
            nRelationship = 14,
            nEducational_Level = 15,
            nNationality = 20,
            nRace = 21,
            nReligion = 22,
            nYesNo = 25,
            nTypeEmployee = 7,
            nTypeAddress = 23,
        }

        public enum DataLang
        {
            Thai = 56,
            Eng = 57
        }

        public enum MenuPermission
        {
            NotAllow = 0,
            ReadOnly = 1,
            Control = 2,
        }

        public enum PositionID
        {
            PM = 1,
            Sale = 2,
            SA = 3,
            BA = 4,
            Co = 5,
            Dev = 6,
            Test = 7,
            Admin = 8,
            HR = 9
        }
        #endregion

    }
}