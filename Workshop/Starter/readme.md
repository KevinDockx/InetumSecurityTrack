# Getting started
- Run update-database on the ShopEtum.MinimalApi project to create the API's database on your local DB instance.  
- To view tokens, jwt.io is your friend.

# Workshop description

You're looking at a variation of our ShopEtum demo application.  The client application is simplified: it consists of one view with two buttons on it, with some JavaScript that'll be executed when you click a button (the JS can be found in site.js).  Clicking the first button should result in the user's claims being returned.  Clicking the second button should return a list of products.  

The products are exposed via a remote API: ShopEtum.MinimalApi, endpoint /products.

No security has been implemented anywhere.  

- Integrate with an identity provider, using the code flow + PKCE.  You can use the login button to log in, or make the full controller only accessible when authorized.  Make sure you use the best practice for JavaScript-based applications: a BFF.  If you're not used to working with JavaScript and/or you think you're more likely to use another technology (regular MVC, Blazor, ...), feel free to use that as a client. We haven't provided it, but start -> new project is just a few clicks away.  We're here to help. 

	
	We've set up Entra ID as such:
	- User: inetumworkshop@kevindockxgmail.onmicrosoft.com
	- Password: ask Kevin :)
	- Well-known document: https://login.microsoftonline.com/5c154a7e-0c13-4f92-8531-e3f4d8fbeae9/v2.0/.well-known/openid-configuration
	- Authority: https://login.microsoftonline.com/5c154a7e-0c13-4f92-8531-e3f4d8fbeae9/v2.0
	- Client id: 50e0b5f1-add4-46fb-a878-78fac0c4ed2e (inetumworkshop-webclient)
	- Client secret: PtY8Q~j1vApzSU5yw1NfN9DMCTYyxGJTNZDJ~dms
	- Scope for API access: api://f694f85a-0c1a-4da0-a0fb-83618e0615a6/shopetum.fullaccess (inetumworkshop-api)
	- More granular scopes: api://f694f85a-0c1a-4da0-a0fb-83618e0615a6/shopetum.read api://f694f85a-0c1a-4da0-a0fb-83618e0615a6/shopetum.write
	- Scope for downstream API access: TODO (inetumworkshop-downstreamapi)
	
DO NOT change the port number of the starter applications.  Entra ID is configured to only return tokens to the current localhost:theport URI(s), at endpoint /signin-oidc (the default).
			
We suggest using MS's OIDC middleware (Microsoft.AspNetCore.Authentication.OpenIdConnect) to integrate with the IDP, but if you're free to choose.  If you really want to make us unhappy, you can even use the Microsoft Authentication Library (MSAL).

- Write out the name of the current logged-in user next to the login button
- Make sure the buttons on the main view don't show up if the user isn't authenticated 
- Add a local endpoint to the client app, and call that securely.  Use an approach of choice to create the endpoint (minimal API, MVC).  The endpoint must return the current user's claims.  Show them on the page.
- Make sure that the user's Name in the ClaimsIdentity is coming from the "name" claim in the token.
- Improve the security of your cookie: set the Secure attribute on it, and make it HttpOnly.  Consider what the best SameSite mode is.  
- Protect your application against XSS with a CSP
- Secure the API: access should only be granted if an access token with audience api://b511b135-f73d-433a-8cac-fcc14ccf82c3 or scope shopetum.fullaccess is passed through. Consider which approach to take: audience-based validation or scope-based validation.

TIP: if you don't add a valid API/resource scope, you will still get an access token.  Its audience value will be "00000003-0000-0000-c000-000000000000".  This is an access token returned by default by Entra ID, used to access the MS Graph.  It must not be used for securing access to your own API!

- Implement the get products functionality. This must securely call the ShopEtum API, endpoint GET /products.  Think about the BFF pattern in this approach: the JavaScript client must never directly call the API.  While doing that, figure out a way to pass through access tokens, and to refresh them if needed.  You're free to use any approach you want, from extending HttpClient, potentially writing your own HttpMessageHandler, using a helper package like Duende's access token management package, combine (or don't combine) that with Yarp, or even use the full-blown Duende.BFF package. 

### Additional things to do: 
- Improve your security by storing the secrets in Azure KeyVault.  The KeyVault is named "sectrack-kv-dev-we", the Inetum Demo Workshop User has permission to read secrets from it, and the client secret is stored with key "InetumDemoClientSecret".
- Play around with authorization policies.  For example, create one in which you only allow access to products for users who live in a certain country. 
- We've provided a downstream API that will return a fake "ReallyTrulyMostUpToDateProductPrice". Secure this one as well, and call this one from the ShopEtum API.  Two approaches can be taken: 
	- Imagine that the downstream API doesn't need to know who you are (you = the initial user): use the client credentials flow.  You can use these settings: 
	- Challenge yourself, take the fancy appraoch, and imagine that the downstream API needs to know who you are: for this, you can use an "on behalf of"  (as it is called in Entra ID) or "token exchange" (as the original flow that one is based on is called) flow.  Note that this is only supported in Entra ID, not in Azure AD B2C.  You can use these settings:
- Make a theoretical analysis: think about how you can implement vertical and horizontal access control (google it if you don't know what that is ;-)).  