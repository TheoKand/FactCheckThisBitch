FROM 3.1.416-nanoserver-20H2

COPY /bin/Debug/netcoreapp3.1/publish App/
WORKDIR /App
ENTRYPOINT ["dotnet", "BrowserUtil.dll"]