namespace OcelotApiGatewayDemo;

public class TokenPreferences
{
    /// <summary>
    ///     Property to get and set the audience of the event.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    ///     Gets or sets the issuer of the token.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    ///     Property to get and set the security key.
    /// </summary>
    public string SecurityKey { get; set; }

    /// <summary>
    ///     Gets or sets the expiration time of the access token in minutes.
    /// </summary>
    public int AccessTokenExpiration { get; set; }

    /// <summary>
    ///     Gets or sets the expiration time for the refresh token in minutes.
    /// </summary>
    public int RefreshTokenExpiration { get; set; }
    /// <summary>
    ///    Gets or sets the expiration time for the long access token in minutes.
    /// </summary>
    public int LongAccessTokenExpiration { get; set; }
    /// <summary>
    ///    Gets or sets the expiration time for the long access token in minutes.
    /// </summary>
    public int ForgotPasswordTokenExpiration { get; set; }
}


