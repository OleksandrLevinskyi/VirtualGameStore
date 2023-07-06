# Virtual Game Store
This project is a system for selling games.

User Documentation (for more information on how the app works): [https://bit.ly/3O3sKHy](https://bit.ly/3O3sKHy)

Gallery (app screenshots from User Documentation): [https://bit.ly/44vqwGf](https://bit.ly/44vqwGf)

## Local environment variables
* Duplicate `appsettings.Development.Example.json` and rename it to `appsettings.Development.json`
* Add relevant missing local variables

## Setup email
Sending emails is disabled by default. To enable it, set the following variables in `appsettings.Development.json`:
* `"IsEmailEnabled": true`
* Under `"EmailSettings"`
	* `"SenderEmail": "<your gmail>"`
	* `"Password": "<your google app password>"`

### Getting a Google app password
To use you gmail as an email sender, you'll need to have a Google app password
* Visit https://security.google.com/settings/security/apppasswords
* Add a password. You can select any app and any device on the dropdowns as this is a global password.
