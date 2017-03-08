rmdir /q/s bin
md bin
md bin\templates
zip -r -j bin\Templates\AbstractBaseClass.zip src\AbstractBaseClass\*.* 
zip -r -j bin\Templates\BaseControl.zip src\BaseControl\*.* 
zip -r -j bin\Templates\BasicReport.zip src\BasicReport\*.* 
zip -r -j bin\Templates\BasicTextIO.zip src\BasicTextIO\*.* 
zip -r -j bin\Templates\BusinessProcess.zip src\BusinessProcess\*.* 
zip -r -j bin\Templates\Entity.zip src\Entity\*.* 
zip -r -j bin\Templates\ReportLayout.zip src\ReportLayout\*.* 
zip -r -j bin\Templates\Type.zip src\Type\*.* 
zip -r -j bin\Templates\UIController.zip src\UIController\*.* 
zip -r -j bin\Templates\WebProcess.zip src\WebProcess\*.* 
xcopy Shortcuts\*.*  bin\Shortcuts\*.* 
cd bin
zip -r Templates-and-snippets.zip templates\ Shortcuts\
cd..