using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_Language
{
    /// <summary>
    /// Language Code
    /// </summary>
    public string sLanguageCode { get; set; } = null!;

    /// <summary>
    /// Language Flag
    /// </summary>
    public string sLanguageFlag { get; set; } = null!;

    /// <summary>
    /// Language Value
    /// </summary>
    public string sLanguageName { get; set; } = null!;

    /// <summary>
    /// Language Description
    /// </summary>
    public string? sLanguageDescription { get; set; }

    /// <summary>
    /// 1 = Main Language, 0 = Not Main Language
    /// </summary>
    public bool isMainLanguage { get; set; }

    /// <summary>
    /// 1 = Main Email Language, 0 = Not Main Email Language
    /// </summary>
    public bool isMainEmailLanguage { get; set; }

    /// <summary>
    /// Sort Order
    /// </summary>
    public int nSortOrder { get; set; }

    /// <summary>
    /// 1 = Active, 0 = Inactive
    /// </summary>
    public bool isActive { get; set; }

    /// <summary>
    /// Native Name
    /// </summary>
    public string? sNativeName { get; set; }

    /// <summary>
    /// Three Letter Code
    /// </summary>
    public string? sThreeLetterCode { get; set; }
}
