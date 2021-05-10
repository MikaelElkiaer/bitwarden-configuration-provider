# Unlocking Bitwarden

First of all, it is necessary to sign in:
```
bw login
```

While running the application and unlocking is required, this will be prompted by the running program.

In order to avoid getting prompted, Bitwarden CLI can be unlocked by updating `~/.bw-session-key.tmp` with a session key - acquired by the command:
```
bw unlock
```

This can be also be done semi-automatically via:
```
bw unlock --raw >> ~/.bw-session-key.tmp
```

# Types of secrets

Bitwarden is not exactly meant for secrets, like for instance HashiCorp's Vault.
Besides credential-related secrets, Bitwarden has a generic type 'note' which is not much more than a name and a value.
I've decided to utilize this in order to support two types of secrets:

* Single value secret
* .env file secret

The single value secret is simply transformed into a single key-value-pair, with the Bitwarden note name as key, and the note itself as a value.

The .env file secret is treated as a standard .env file, expecting a key-value-pair per line of the note, each line in the format `KEY=VALUE`.

