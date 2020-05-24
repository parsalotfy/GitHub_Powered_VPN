FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine

RUN apk update
RUN apk upgrade
RUN apk add --no-cache git

ENTRYPOINT git clone https://parsalotfy:$MY_GIT_TOKEN@github.com/parsalotfy/GitHub_Powered_VPN.git && git config --global user.email parsalotfy@outlook.com && cd GitHub_Powered_VPN/L2TP_Fetcher && dotnet run && git add --all && git commit -m $( date '+%F_%H:%M:%S' ) && git push
