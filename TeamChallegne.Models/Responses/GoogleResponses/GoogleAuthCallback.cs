﻿namespace TeamChallenge.Models.Responses
{
    public class GoogleAuthCallback
    {
        public OAuthGoogleResponse AuthGoogleResponse { get; set; }
        public TokenResponse TokenResponse { get; set; }
    }
}
