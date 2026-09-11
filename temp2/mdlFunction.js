//function confirmation()
//{
//  if (!(confirm("Are you sure to logout  this session ?")))
//     return(false)
//     else
//     {
//     location.href = "logout.aspx"
//     }
//}

function select_deselectAll(chkVal,idVal)
{
  var frm = document.forms[0];
  // Loop through all elements
  for (i=0;i<frm.length;i++){
     // Look for our Header Template's checkbox
      if (idVal.indexOf('chkBxHeader') != -1) {
        // Check if main checkbox is checked, then select or deselect datagrid checkboxes
        if(chkVal==true) {
           frm.elements[i].checked=true;
        } else {
           frm.elements[i].checked=false;
        }
        
        // Work here with the Item template's multiple checkboxes
        }
   else if (idVal.indexOf('chkBxSelect') != -1) {
        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
        if (frm.elements[i].checked==false) {
           frm.elements[i].checked=false; // Uncheck main select all checkbox
        }
     }
}
}

// -----------------------------------------------------------------------------
// Disable right click script
// -----------------------------------------------------------------------------
//var nav = navigator.appName;
//var isIE = nav == "Microsoft Internet Explorer";
//var isNetscape = nav == "Netscape";

//function DisableRbtn(button)
//{
//    if (isIE)
//    {
//         event.returnValue = false;
//         return false;
//    }
//    if (isNetscape)
//    {
//         if(button.which==3)
//              return false;
//    }
//}

//if (isNetscape)
//{
//    document.captureEvents(Event.MOUSEDOWN);
//    document.captureEvents(Event.MOUSEUP);
//}
//document.onmousedown = DisableRbtn;
//document.onmouseup = DisableRbtn;
//document.oncontextmenu = DisableRbtn;
// -----------------------------------------------------------------------------