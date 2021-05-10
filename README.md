# Unlocking Bitwarden

First of all, it is necessary to sign in:
```
bw login
```

Then, if unlocking is required, this will be prompted by the running program.

In order to avoid getting prompted, Bitwarden CLI can be unlocked by setting the `.bw-session-key.tmp` with a session key aqcuired via the command:
```
bw unlock
```

This can be done semi-automatically via:
```
bw unlock --raw >> ~/.bw-session-key.tmp
```