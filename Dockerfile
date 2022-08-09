FROM mcr.microsoft.com/dotnet/runtime:6.0
RUN apt-get update \
    && apt-get install curl -y \
    && apt-get install xz-utils -y \
    && apt-get install nano -y \
    && apt-get install flip -y \
    && apt-get clean
    
RUN mkdir /ffmpeg && \
    cd /ffmpeg && \
    curl https://johnvansickle.com/ffmpeg/releases/ffmpeg-release-amd64-static.tar.xz -o ffmpeg.tar.xz -s && \
    tar -xf ffmpeg.tar.xz && \
    mv ffmpeg-*-amd64-static/ffmpeg /usr/bin && \
    cd / && \
    rm -rf ffmpeg
COPY . .
RUN chmod a+x Commands/*
RUN cd /Commands && flip -u *
ENTRYPOINT ["dotnet", "telegram_audio_bot.dll"]