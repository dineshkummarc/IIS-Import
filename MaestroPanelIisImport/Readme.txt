IIS Import Tool
IIS Import, IIS 7.5 üzerindeki web sitelerini MaestroPanel'e aktarmanýza yarayan ufak ve kullanýþlý yardýmcý bir araçtýr. Mevcut IIS 7.5 Web sunucunuzdaki sitelerin fqdn standartlarýna uygun isimleri bulur ve belirtilen Plan'a göre domainleri oluþturur.

Ýþlem sonunda "iisimport.log" isminde bir text dosyasý çalýþtýðý dizinde oluþturur. Bu dosyanýn içinde otomatik olarak verilen þifre ve tüm iþlemlerin sonuçlarý "tab delimited" þeklinde formatlanarak loglanýr.

Kullaným¶
IIS Import, Console uygulamasý olarak tasarlanmýþtýr ve komut satýrýndan kolaylýkla çalýþtýrýlabilir.

iisimport.exe -key 1_1e9f1131d7cc44fd8c5df1aca0155937 -host -plan GOLDPLAN contoso.com -port 9715 -ssl false


Parametreler

    -key MaestroPanel üzerinden oluþturacaðýnýz API anahtarýný belirtmeniz için parametre. String tipindedir.
    -host Web Management Service'in alanadýný belirtmeniz için parametre. Steing tipindedir.
    -plan Web Management Service'in üzerinde oluþturduðunuz Domain Planýnýn alias kýsmýnda belirttiðiniz isimi girebileceðiniz parametre String tipindedir.
    -port Web Management Service'in port numarasýný belirtmeniz için parametre. Defult port 9715 dir. Integer tipindedir.
    -ssl Web Management Service'e ulaþýrken https kullanýlýp, kullanýlmayacaðýný belirleyeceðiniz parametredir. true veya false deðeri alabilir. Boolean tipindedir.


Lokasyon
Bu araç MaestroPanel Agent ile birlikte gelmektedir;

%MaestroPanelPath%bin\iisimport.exe

dizini altýndadýr. 