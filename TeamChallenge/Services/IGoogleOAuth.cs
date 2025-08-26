﻿using TeamChallenge.Models.Responses;

namespace TeamChallenge.Services
{
    public interface IGoogleOAuth
    {
        public IDataResponse<string> GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange, string state);

        public Task<IDataResponse<GoogleAuthCallback>> GetGoogleAuthCallback(string code);

        void GenerateCodeVerifierState(out string codeVerifier, out string state, out string codeChallenge);

    }
}
