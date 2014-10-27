// requires Windows 2000 Service Pack 3, Windows 98, Windows 98 Second Edition, Windows ME, Windows Server 2003, Windows XP Service Pack 2
// requires internet explorer 5.0.1 or higher
// requires windows installer 2.0 on windows 98, ME
// requires Windows Installer 3.1 on windows 2000 or higher
// http://www.microsoft.com/downloads/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5

[CustomMessages]
Libreoffice_title=Libre office

Libreoffice_size=214 MB


[Code]
const
	Libreoffice_url = 'http://download.documentfoundation.org/libreoffice/stable/4.3.2/win/x86/LibreOffice_4.3.2_Win_x86.msi';


procedure Libreoffice();
begin
//	if (not netfxinstalled(NetFx20, '')) then
//		AddProduct('Libreoffice' + GetArchitectureString() + '.msi',
//			'/passive /norestart /lang:ENU',
//			CustomMessage('Libreoffice_title'),
//			CustomMessage('Libreoffice_size'),
//			GetString(Libreoffice_url, Libreoffice_url_x64, Libreoffice_url_ia64),
//			false, false);

AddProduct('LibreOffice_4.3.2_Win_x86.msi',
			'/passive /norestart',
			CustomMessage('Libreoffice_title'),
			CustomMessage('Libreoffice_size'),
			Libreoffice_url,
			false, false);
end;