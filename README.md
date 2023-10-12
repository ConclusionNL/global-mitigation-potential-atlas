# global-mitigation-potential-atlas
The Global Mitigation Potential Atlas - a web based portal to provide global insight in the potential for reducing carbon emissions through governmental policies and application of technologies

# Installation

## HTTPS on Frontend
- Go into "Res/mkcert" and run the `install.bat`.

## Updating the root CA
- In case the root CA has expired, go into "Res/mkcert".
- Remove the `rootCA.pem` and `rootCA-key.pem`.
- Run the `generate.bat`.

## Updating certificates
- In case the certificates have expired, go into "Res/mkcert".
- Run the `generate.bat`.
- Copy `localhost.pem` and `localhost-key.pem` to `Src/Frontend/gmpa-web/certs`.