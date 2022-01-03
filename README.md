This is a custom configuration provider for .NET Core applications already utilizing `Microsoft.Configuration`.
It utilizes the Bitwarden CLI to retrieve secure notes and insert them as configuration keys in the running application.
It is primarily meant to be used as a means of holding secrets during development, and it is it therefore not currently recommended to use in a production setting.

# Installing Bitwarden CLI
The details can be found here: https://bitwarden.com/help/article/cli/#download-and-install

Quick install instructions for the most common package distributors:

* NPM: `npm install -g @bitwarden/cli`
* Homebrew: `brew install bitwarden-cli`
* Chocolatey: `choco install bitwarden-cli`
* Snap: `sudo snap install bw`
* AUR: `yay -S bitwarden-cli`

# Unlocking Bitwarden via the CLI
Before the configuration provider can be taken into use, it is necessary to sign in:
```
bw login
```

The first time an application with the configuration provider is run, it will prompt for a password in order to start a session.
The configuration provider will save the session key in `~/.bw-session-key.tmp` (`/home/USER` on Unix and `C:/Users/USER` on Windows).

## Unlocking outside the running application 
In order to avoid getting prompted, the configuration provider can be unlocked by updating `~/.bw-session-key.tmp` with a session key - acquired by the command:
```
bw unlock
```

This can be also be done semi-automatically via:
```
bw unlock --raw >> ~/.bw-session-key.tmp
```

## Locking
This can be done by throwing away the session key, i. e. deleting `~/.bw-session-key.tmp`.
Alternativaly, all session keys can be invalidated via:
```
bw lock
```

# Usage
Bitwarden is not primarily meant for secret management, like for instance HashiCorp's Vault.
However, besides credentials types (Login/Card/Identity), Bitwarden also has a type 'Secure note'.
A secure note consists of a name, some fields, and a 'notes' property.
If fields are omitted, a secure note is basically a key-value-pair.

## Default usage
By default, the configuration provider will replace key-values which refer to a secret found in Bitwarden.
If the configuration provider is enabled without any configuration, it will only alter already defined configuration keys, set by other configuration providers such as JsonFile or EnvironmentVariables.
It will update any keys, which has a certain value - in the following format:
```
{PREFIX}{NAME}[.{FIELDNAME}]
```

The prefix is by default `bw:`, the name refers to the name of a Bitwarden secret, and the field name is optionally set to get the value of a specific field.
If there is no field name, or it is set to `notes`, it will refer to the notes property of the secret.
Note the use of `.` as a field selector which prohibits the use of `.` in secret names.

## More complex configurations
The configuration provider supports the following types of secrets:
* Env file secret
* Single value secret
* Fields secret

The single value secret is simply transformed into a single key-value-pair, with the name as key, and the note itself as a value.

The .env file secret is treated as a standard .env file, expecting a key-value-pair per line of the note, each line in the format `KEY=VALUE`.

A fields secret takes all fields and turn them into a key in the format `{NAME}{FieldSeperator}{FIELDNAME}`, where notes is reserved and will refer to the note property.

