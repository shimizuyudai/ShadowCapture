SET APP1=.\Front\Front.exe
SET APP2=.\Rear\Rear.exe 
SET OPTION = -screen-width 300 -screen-height 300 -popupwindow

START "" "%APP1%" -screen-width 300 -screen-height 300 -popupwindow
timeout 5
START "" "%APP2%" -screen-width 300 -screen-height 300 -popupwindow