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

