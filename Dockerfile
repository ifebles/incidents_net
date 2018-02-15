FROM microsoft/iis:10.0.14393.206
SHELL ["powershell"]

RUN Install-WindowsFeature NET-Framework-45-ASPNET ; \
    Install-WindowsFeature Web-Asp-Net45


#inetpub\wwwroot
COPY Publish Publish

#RUN Remove-WebSite -Name 'Default Web Site'

#RUN New-WebSite -Name 'checkincidents' -Port 80 \
#    -PhysicalPath 'c:\\Publish' -ApplicationPool '.NET v4.5'

# RUN New-WebSite -Name 'checkincidents' -Port 80 -PhysicalPath 'c:\\Publish' -ApplicationPool '.NET v4.5'

#This.part.right.here.was.just.... bboi...
# WORKDIR c:\\Windows\System32\inetsrv

RUN cd c:\\Windows\System32\inetsrv; .\appcmd.exe add app /site.name:\"Default Web Site\" /path:/checkincidents /physicalPath:c:\Publish

EXPOSE 80