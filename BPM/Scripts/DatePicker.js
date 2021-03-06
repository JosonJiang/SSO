var MNoneText,MLangID,MImagePath;
var MblnDisableNone,MblnIsShown;
var MDateRef,MSelectorRef,MCalendarArea;
var MdtToday,MdtSeasonStart,MdtSeasonEnd;
var MintFormatMode=1;
var MintOneMinute=60*1000;
var MintOneHour=MintOneMinute*60;
var MintOneDay=MintOneHour*24;
var MSltDay=0;
var MSltMonth=0;
var MSltYear=0;
var MSltHour=0;
var MSltMinutes = 0;
var MintCrtMonth=0;
var MintCrtYear=0;
var MSplictChar="-";
var MDoPostBack=false;

if(navigator.userAgent.indexOf("Gecko")>0)
{
	MBrowser="Gecko";
	document.onclick=MHideDateSelector
}
else
{
	MBrowser="IE";
	document.onclick=function()
	{
		MHideDateSelector(event)
	}
}

if((typeof(HTMLElement)!="undefined")&&(!HTMLElement.prototype.insertAdjacentHTML))
{
	HTMLElement.prototype.insertAdjacentHTML=function(where,htmlStr)
	{
		var r=this.ownerDocument.createRange();
		r.setStartBefore(this);
		var parsedHTML=r.createContextualFragment(htmlStr);
		this.appendChild(parsedHTML);
	}
}

function HTMLShowDateSelector(DateRef,EventRef,DisableNone,LangID,ImagePath)
{
	ShowDateSelector(DateRef,EventRef,DisableNone,LangID,ImagePath,false)
}

function MShowDateSelector(DateRef,EventRef,LangID,ImagePath,NeedPBack)
{
	ShowDateSelector(DateRef,EventRef,false,LangID,ImagePath,NeedPBack)
}

function ShowDateSelector(DateRef,EventRef,DisableNone,LangID,ImagePath,NeedPBack)
{
	if(MblnIsShown)
	{
		return
	}
	else
	{
		MblnIsShown=true
	}
	if(document.getElementById)
	{	
		if(!MSelectorRef)
		{
			MdtToday=new Date();
			MdtToday.setHours(0,0,0,0);
			MWriteSelectorHTML();
			MSelectorRef=document.getElementById("MdateSelector");
			MCalendarArea=document.getElementById("McalendarArea");			
		}
		
		//		alert(MSelectorRef.outerHTML);
		//		alert(MCalendarArea.outerHTML);
	
		
		MDateRef=DateRef;
		MblnDisableNone=DisableNone;
		MLangID=LangID;
		MImagePath=ImagePath;
		MDoPostBack=NeedPBack;
		switch(MLangID)
		{
			case"FR":
				MNoneText='';
				break;
			case"CN":
				MNoneText='';
				break;
			case"US":
				MNoneText='';
				break;
			default:
				MNoneText='';
		}
		//设置可选置的日期范围，有需要时可以启用下面被注销的两行
		//MdtSeasonStart=new Date(MdtToday.getTime()-(MintOneDay*184));
		//MdtSeasonEnd=new Date(MdtToday.getTime()+(MintOneDay*182));

		MSltDay=0;
		var initValue;
		if(MDateRef.value==MNoneText)
		{
			if(MDateRef.defaultValue==MNoneText)
			    initValue = MDateToString(MdtToday);
			else
			    initValue = MDateRef.defaultValue;
		}
		else
		{
		    initValue = MDateRef.value;
		}

        var arrCrtDate = initValue.split(' ');
        if(arrCrtDate.length >= 1)
        {
            var arrDate = arrCrtDate[0].split(MSplictChar);
		    MSltYear=arrDate[0];
		    MSltMonth=arrDate[1]-1;
		    MSltDay=arrDate[2];
		}
        if(arrCrtDate.length >= 2)
        {
            
            var arrTime = arrCrtDate[1].split(':');
		    MSltHour=arrTime[0];
		    MSltMinutes=arrTime[1];
		}
			
		MintCrtMonth=parseInt(MSltMonth,10);
		MintCrtYear=parseInt(MSltYear,10);
		
		MCalendarArea.innerHTML=MCreateCalendarArea();
		if(MBrowser=="Gecko")
		{
			MSelectorRef.style.left=EventRef.clientX-90;
			MSelectorRef.style.top=EventRef.clientY+8;
		}
		else
		{
			//MSelectorRef.style.left=EventRef.clientX-EventRef.offsetX-86+document.body.scrollLeft;
			//MSelectorRef.style.top=EventRef.clientY-EventRef.offsetY+16+document.body.scrollTop;
			AdjustDropPos(DateRef,MSelectorRef);
		}
				
		MSelectorRef.style.visibility="visible";
		MSelectorRef.style.display="block";
		EventRef.cancelBubble = true;
	}
}

function AdjustDropPos(refEle,dropEle)
{
	var left;
	var top;

	left = refEle.offsetLeft + 2;
	top = refEle.offsetTop + refEle.offsetHeight + 2;
	
	while(1)
	{
		if( refEle == null )
			break;
		
		if( refEle.tagName == "BODY" )
			break;

		refEle = refEle.offsetParent;
		if( refEle == null )
			break;

		left += refEle.offsetLeft;
		top += refEle.offsetTop;
	}
	
	dropEle.style.left = left;
	dropEle.style.top = top;
}

function MHideDateSelector(TheEvent)
{
	if(MSelectorRef)
	{
		if(MBrowser=="Gecko")
		{
			if(TheEvent)
			{
				var ThisIcon="MdsIcon_"+MDateRef.name;
				var rel=TheEvent.target;
				while(rel)
				{
					if((rel.id=="MdateSelector")||(rel.id==ThisIcon))
					{
						break
					}
					else
					{
						rel=rel.parentNode
					}
				}
			}
			if(!rel)
			{
				MSelectorRef.style.display="none";
				MblnIsShown=false
			}
			return;
		}
		else
		{
			if((TheEvent))
			{
				if((TheEvent.clientX+document.body.scrollLeft>MSelectorRef.style.posLeft+1)&&(TheEvent.clientX+document.body.scrollLeft<MSelectorRef.style.posLeft+MSelectorRef.style.posWidth+10)&&(TheEvent.clientY+document.body.scrollTop>MSelectorRef.style.posTop+1)&&(TheEvent.clientY+document.body.scrollTop<MSelectorRef.style.posTop+MSelectorRef.offsetHeight+2))
				{
					return
				}
				if((TheEvent.clientX+document.body.scrollLeft>MSelectorRef.style.posLeft+81)&&(TheEvent.clientX+document.body.scrollLeft<MSelectorRef.style.posLeft+99)&&(TheEvent.clientY+document.body.scrollTop>MSelectorRef.style.posTop-17)&&(TheEvent.clientY+document.body.scrollTop<MSelectorRef.style.posTop))
				{
					return
				}
			}
			MSelectorRef.style.display="none";
			MblnIsShown=false;
		}
	}
	else
	{
		MSelectorRef=false
	}
}

function MCreateCalendarArea()
{
	switch(MLangID)
	{
		case"FR":
			var MStrToday="Today";
			var MNoneValue="";
			break;
		case"CN":
			var MStrToday="Today";
			var MNoneValue="";
			break;
		default:
			var MStrToday="Today";
			var MNoneValue="";
	}
	var dtFirstOfMonth=new Date(MintCrtYear,MintCrtMonth,1);
	switch(dtFirstOfMonth.getDay())
	{
		case 0:
			var OffsetDays=6;
			break;
		case 1:
			var OffsetDays=7;
			break;
		default:
			var OffsetDays=dtFirstOfMonth.getDay()-1;
	}
	var dtCalendarStart=new Date(dtFirstOfMonth.getTime()-(MintOneDay*OffsetDays));
	var dtCalendarEnd=new Date(dtCalendarStart.getTime()+(MintOneDay*41));

	MHtmlCode="<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' CLASS='Mcalendar'>";
	MHtmlCode+="<TR CLASS='McalendarTitles' ALIGN='center'>";

	for(var i=0;i<=6;i++)
	{
		MHtmlCode+="<TD WIDTH='22'>"+MDefWeek[i]+"</TD>"
	}
	MHtmlCode+="</TR>";
	for(var i=0;i<=41;i++)
	{
		if(i%7==0)
		{
			MHtmlCode+="<TR ALIGN='center'>"
		}
		var StyleString="";
		var dtTheDay=new Date(dtCalendarStart.getTime()+(MintOneDay*i));
		if(dtTheDay.getTime()==MdtToday.getTime())
		{
			if((dtTheDay.getMonth()==MSltMonth)&&(dtTheDay.getDate()==MSltDay)&&(dtTheDay.getFullYear()==MSltYear))
			{
				StyleString+="background-image: url("+MImagePath+"today_selected.gif); background-repeat:no-repeat;"
			}
			else
			{
				StyleString+="background-image: url("+MImagePath+"today.gif); background-repeat:no-repeat;"
			}
		}
		else if((dtTheDay.getMonth()==MSltMonth)&&(dtTheDay.getDate()==MSltDay)&&(dtTheDay.getFullYear()==MSltYear))
		{
			StyleString+="background-image: url("+MImagePath+"selected.gif); background-repeat:no-repeat;"
		}
		if(dtTheDay.getMonth()!=MintCrtMonth)
		{
			var LinkClass="MnotInMonth"
		}
		else
		{
			var LinkClass="MinMonth"
		}
		if(((MdtSeasonStart)&&(MdtSeasonEnd))&&((dtTheDay<MdtSeasonStart)||(dtTheDay>MdtSeasonEnd)))
		{
			MHtmlCode+="<TD CLASS='MoutOfRange' STYLE='"+StyleString+"'>"+dtTheDay.getDate()+"</TD>"
		}
		else
		{
			MHtmlCode+="<TD STYLE='"+StyleString+"'><A HREF='Javascript: void MSetDate(\""+MDateToString(dtTheDay)+"\")' CLASS='"+LinkClass+"'>"+dtTheDay.getDate()+"</A></TD>"
		}
		if(i%7==6)
		{
			MHtmlCode+="</TR>"
		}
	}

	MHtmlCode+="</TABLE>";

	var MSelectOption="";
	var Selected="";
	for(var i=0;i<=11;i++)
	{
		if(i==MintCrtMonth)
		{
			Selected=" SELECTED"
		}
		else
		{
			Selected=""
		}
		MSelectOption+='<OPTION'+Selected+'>'+MDefMonth[i]+'</OPTION>';
	}
	
	var timeRow = "";
	if(MDateRef.XSubType != "date")
	{
	    var HourOption="";
	    for(var i=0;i<=23;i++)
	    {
		    if(i==MSltHour)
		    {
			    Selected=" SELECTED"
		    }
		    else
		    {
			    Selected=""
		    }
		    
		    var str = i <= 9 ? ('0'+i):(i); 
		    HourOption+='<OPTION'+Selected+' value='+str+'>'+str+'</OPTION>';
	    }
	    
	    var MinuteOption="";
	    var MStep = 1;
	    if (MDateRef.XSubType == "timeminutes30")
	    {
	        MStep = 30;
	    }
	    else if (MDateRef.XSubType == "timeminutes15")
	    {
	        MStep = 15;
	    }
	    else
	    {
	        MStep = 1;
	    }

	    for(var i=0;i<=59;i+=MStep)
	    {
		    if(i==MSltMinutes)
		    {
		        Selected=" SELECTED"
		    }
		    else
            {
	            Selected=""
		    }
		    
		    var str = i <= 9 ? ('0'+i):(i);
		    MinuteOption+='<OPTION'+Selected+' value='+str+'>'+str+'</OPTION>';
	    }
	
	    var date = MDateToString(new Date(MSltYear,MSltMonth,MSltDay));
	    timeRow = '<TR HEIGHT="22" CLASS="MbuttonsRow">'+'<TD COLSPAN="5">'+'<TABLE BORDER="0" CELLPADDING="0" CELLSPACING="0" WIDTH="100%"><TR><TD ALIGN="left"><TD ALIGN="left" ID="MdateToday"><input type="text" id="sys_dp_day" class="MDateInput" value='+date+'><SELECT id="sys_dp_hour" class="MHourMinuteSel" onclick="MChangeHour();">' + HourOption + '</SELECT><SELECT id="sys_dp_minutes" class="MHourMinuteSel" onclick="MChangeMinutes();">' + MinuteOption + '</SELECT></TD>'+'<TD ALIGN="right"><INPUT TYPE="image" SRC="'+MImagePath+'/but_ok.gif" WIDTH="18" style="height:18px" HSPACE="1" onClick="MSetDateAndClose();"></TD>'+'</TR>'+'</TABLE>'+'</TD>'+'</TR>'
	}
	
	MHtmlCode='<TABLE BORDER="0" CELLPADDING="1" CELLSPACING="0" WIDTH="100%">'+'<TR style="height:22px" CLASS="MbuttonsRow">'+'<TD WIDTH="30" ALIGN="left"><INPUT TYPE="image" SRC="'+MImagePath+'but_prev.gif" WIDTH="18" style="height:18px" HSPACE="1" onClick="MAdvanceDate(-1)"></TD>'+'<TD><SELECT NAME="McurrentMonth" CLASS="MmonthRolldown" onChange="MChangeMonth(this.selectedIndex + 1)">'+MSelectOption+'</SELECT></TD>'+'<TD style="padding-right:1px;"><INPUT TYPE="text" NAME="McurrentYear" VALUE="'+MintCrtYear+'" class="MyearInput" onkeyup="MChangeYear(this.value);"></TD>'+'<TD><INPUT TYPE="image" SRC="'+MImagePath+'but_yeard.gif" WIDTH="18" style="height:9px" onClick="MAdvanceDate(12)"><BR><INPUT TYPE="image" SRC="'+MImagePath+'but_yearu.gif" WIDTH="18" style="height:9px" onClick="MAdvanceDate(-12)"></TD>'+'<TD WIDTH="30" ALIGN="right" style="padding-left:2px;"><INPUT TYPE="image" SRC="'+MImagePath+'but_next.gif" WIDTH="18" style="height:18px" HSPACE="1" onClick="MAdvanceDate(1)"></TD>'+'</TR>'+'<TR HEIGHT="133" BGCOLOR="#FFFFFF"><TD COLSPAN="5" ALIGN="center">'+MHtmlCode+'</TD></TR>'+ timeRow + '<TR HEIGHT="22" CLASS="MbuttonsRow">'+'<TD COLSPAN="5">'+'<TABLE BORDER="0" CELLPADDING="0" CELLSPACING="0" WIDTH="100%">' + '<TR>'+'<TD ALIGN="left"><INPUT TYPE="image" SRC="'+MImagePath+MLangID+'/but_today.gif" WIDTH="44" style="height:18px" HSPACE="1" onClick="MSetDate(MDateToString(MdtToday))"></TD>'+'<TD ALIGN="center" ID="MdateToday"> '+MDateToString(MdtToday)+'</TD>'+'<TD ALIGN="right"><INPUT TYPE="image" SRC="'+MImagePath+MLangID+'/but_none.gif" WIDTH="44" style="height:18px" HSPACE="1" onClick="MSetDate(\''+MNoneValue+'\')"></TD>'+'</TR>'+'</TABLE>'+'</TD>'+'</TR>';
	return MHtmlCode;
}

function MSetDateInternal(TheDate,forceClose)
{
	if((TheDate==MNoneText)&&(MblnDisableNone==true))
	{
		switch(MLangID)
		{
			case"FR":
				alert("Cette d鈚e ne peut pas 阾re 'Aucune'");
				break;
			case"CN":
				alert("日期不能设置为空");
				break;
			default:
				alert("This date cannot be set to 'None'");
		}
		return false;
	}
	var tempArray=TheDate.split(MSplictChar,' ',':');
	var resultingDate=new Date(tempArray[0],tempArray[1]-1,tempArray[2]);
	if(((MdtSeasonStart)&&(MdtSeasonEnd))&&((resultingDate<MdtSeasonStart)||(resultingDate>MdtSeasonEnd)))
	{
		switch(MLangID)
		{
			case"FR":
				alert("Veuillez choisir une d鈚e dans la gamme specifi閑");
				break;
			case"CN":
				alert("请选择设定范围内的日期");
				break;
			default:
				alert("Please select a date in the range specified");
		}
	    return false;
	}
	MSltDay=0;
	
	if(MDateRef.XSubType == 'date' || forceClose == true || TheDate == '')
	{
	    MDateRef.value=TheDate;
	    MDateRef.fireEvent('onchange');
	    MHideDateSelector();
	}
	else
	{
	    document.getElementById("sys_dp_day").value = TheDate;
	    
	    if(TheDate == '')
	    {
	        document.getElementById("sys_dp_hour").value = '';
	        document.getElementById("sys_dp_minutes").value = '';
	    }
	}
	
	if(MDoPostBack)
	{
		document.forms[0].submit();
	}
}

function MSetDate(TheDate)
{
    MSetDateInternal(TheDate,false);
}

function MSetDateAndClose()
{
    var date = document.getElementById("sys_dp_day").value;
    var hour = document.getElementById("sys_dp_hour").value;
    var minutes = document.getElementById("sys_dp_minutes").value;

    var datetime;
    if(typeof(date) == 'string' && date.length != 0)
        datetime = date + ' ' + hour + ':' + minutes;
    else
        datetime = '';
        
    MSetDateInternal(datetime,true);
}

function MAdvanceDate(Adjuster)
{
	if((Adjuster==12)||(Adjuster==-12))
	{
		MintCrtYear=MintCrtYear+(Adjuster/12)
	}
	else
	{
		MintCrtMonth=MintCrtMonth+Adjuster;
		if(MintCrtMonth==-1)
		{
			MintCrtMonth=11;
			MintCrtYear--
		}
		if(MintCrtMonth==12)
		{
			MintCrtMonth=0;
			MintCrtYear++
		}
	}
	MCalendarArea.innerHTML=MCreateCalendarArea();
}

function MChangeYear(year)
{
    if(year.length == 4)
    {
        MintCrtYear = parseInt(year);
	    MCalendarArea.innerHTML=MCreateCalendarArea();
	}
}

function MChangeMonth(Adjuster)
{
	MintCrtMonth=Adjuster-1;
	MCalendarArea.innerHTML=MCreateCalendarArea();
}

function MChangeHour()
{
    var hour = document.getElementById("sys_dp_hour").value;
    MSltHour = parseInt(hour);
    window.event.cancelBubble=true;
}

function MChangeMinutes()
{
    var minutes = document.getElementById("sys_dp_minutes").value;
    MSltMinutes = parseInt(minutes);
    window.event.cancelBubble=true;
}

function MDateToString(TheDate)
{
	if(!TheDate)
	{
		return""
	}
	else
	{
		mYear	= TheDate.getFullYear();
		mMonth	= TheDate.getMonth()<9?"0"+(TheDate.getMonth()+1):(TheDate.getMonth()+1);
		mDate	= TheDate.getDate()<10?"0"+TheDate.getDate():TheDate.getDate();
		return(mYear+MSplictChar+mMonth+MSplictChar+mDate);
	}
}

function MMakeDate(TheDay,TheMonth,TheYear)
{
	return new Date(TheYear,TheMonth-1,TheDay)
}

function MCheckDate(thisDateField,LangID)
{
	if(!LangID)
	{
		LangID=MLangID
	}
	switch(LangID)
	{
		case"FR":
			MNoneText='';
			var FailText="Cette date n'est pas valable";
			break;
		case"CN":
			MNoneText='';
			var FailText="无效的日期";
			break;
		case"US":
			MNoneText='';
			var FailText="Date is not valid";
			break;
		default:
			MNoneText='';
			var FailText="Date is not valid";
	}
	if(thisDateField.value=="")
	{
		thisDateField.value=MNoneText
	}
	if((thisDateField.value!=MNoneText)&&(!MCheckDateFormat(thisDateField.value)))
	{
		alert(FailText);
		thisDateField.value=thisDateField.defaultValue
	}
}

function MCheckDateFormat(thisDate)
{
	if(thisDate.indexOf(MSplictChar)==-1)
	{
		return false
	}
	var ArrayDate=thisDate.split(MSplictChar);
	if(ArrayDate.length!=3)
	{
		return false
	}
	if((isNaN(ArrayDate[0]))||(ArrayDate[0]==""))
	{
		return false
	}
	if((isNaN(ArrayDate[1]))||(ArrayDate[1]==""))
	{
		return false
	}
	if((isNaN(ArrayDate[2]))||(ArrayDate[2]==""))
	{
		return false
	}
	var daysInMonth=new Array(0,31,29,31,30,31,30,31,31,30,31,30,31);
	if((parseInt(ArrayDate[2],10)<1)||(parseInt(ArrayDate[2],10)>daysInMonth[parseInt(ArrayDate[1],10)]))
	{
		return false
	}
	if((parseInt(ArrayDate[1],10)==2)&&(parseInt(ArrayDate[2],10)>MDaysInFebruary(parseInt(ArrayDate[2],10))))
	{
		return false
	}
	if((parseInt(ArrayDate[1],10)<1)||(parseInt(ArrayDate[1],10)>12))
	{
		return false
	}
	return true;
}

function MDaysInFebruary(year)
{
	return(((year%4==0)&&((!(year%100==0))||(year%400==0)))?29:28)
}

function MWriteSelectorHTML()
{
    var selectorHTML;
    selectorHTML = '<div ID="MdateSelector" style="width:190px;">';
    selectorHTML += '<iframe src="" frameborder="0" marginheight="0" marginwidth="0" scrolling="no" style="position:absolute;top:0px;left:0px;width:expression(MdateContainer.clientWidth+2);height:expression(MdateContainer.clientHeight+2);z-index:-1;"></iframe>';
    selectorHTML += '<TABLE BORDER="0" CELLPADDING="0" CELLSPACING="0" id="MdateContainer">'+'<TR><TD ID="McalendarArea"></TD></TR>'+'</TABLE>';
    selectorHTML += '</div>';
   
    //var selectorHTML='<TABLE BORDER="0" CELLPADDING="0" CELLSPACING="0" ID="MdateSelector" STYLE="width:190px">'+'<TR><TD ID="McalendarArea"></TD></TR>'+'</TABLE>';
	document.body.insertAdjacentHTML("BeforeEnd",selectorHTML)
}

function MWriteFieldHTML(FormName,FieldName,FieldValue,FieldWidth,ImagePath,LangID,DisableNone,UseOnClick)
{
	if(!LangID)
	{
		LangID="EN"
	}
	if(!DisableNone)
	{
		DisableNone=false
	}
	if(ImagePath.charAt(ImagePath.length-1)!=MSplictChar)
	{
		ImagePath=ImagePath+MSplictChar
	}
	if(document.getElementById)
	{
		var Mimg1=new Image();Mimg1.src=ImagePath+"today_selected.gif";
		var Mimg2=new Image();Mimg2.src=ImagePath+"today.gif";
		var Mimg3=new Image();Mimg3.src=ImagePath+"selected.gif";
		var Mimg4=new Image();Mimg4.src=ImagePath+"but_prev.gif";
		var Mimg5=new Image();Mimg5.src=ImagePath+"but_yearu.gif";
		var Mimg6=new Image();Mimg6.src=ImagePath+"but_yeard.gif";
		var Mimg7=new Image();Mimg7.src=ImagePath+"but_next.gif";
		var Mimg8=new Image();Mimg8.src=ImagePath+LangID+"/but_today.gif";
		var Mimg9=new Image();Mimg9.src=ImagePath+LangID+"/but_none.gif";
		var ActionString='HTMLShowDateSelector(document.'+FormName+'.'+FieldName+',event,'+DisableNone+',\''+LangID+'\',\''+ImagePath+'\')';
		if(UseOnClick==true)
		{
			var ActionEvent="onMouseDown="+ActionString;
			switch(LangID)
			{
				case"FR":
					var IconAltText="Cliquez ici pour choisir une date";
					break;
				case"CN":
					var IconAltText="单击这里选择日期";
					break;
				default:
					var IconAltText="Click here to select a date";
			}
		}
		else
		{
			var ActionEvent="onMouseOver="+ActionString+" onMouseDown="+ActionString;
			var IconAltText="";
		}
		var formFieldHTML=''+'<TABLE BORDER="0" CELLPADDING="0" CELLSPACING="0" BGCOLOR="#FFFFFF" CLASS="MdateSelect" ID="'+FieldName+'Mtable" WIDTH="'+FieldWidth+'" HEIGHT="22" STYLE="width:'+FieldWidth+'px">'+'<TR>'+'<TD><INPUT TYPE="text" NAME="'+FieldName+'" VALUE="'+FieldValue+'" CLASS="MdateField" SIZE="9" MAXLENGTH="10" onChange="MCheckDate(this,\''+LangID+'\')" READONLY></TD>'+'<TD ALIGN="right"><A HREF="JavaScript: void 0" '+ActionEvent+'><IMG SRC="'+ImagePath+'calendar.gif" style="height:16px" WIDTH="16" HSPACE="3" BORDER="0" ALT="'+IconAltText+'" ID="MdsIcon_'+FieldName+'"></A></TD>'+'</TR>'+'</TABLE>';
		document.write(formFieldHTML);
	}
	else
	{
		var formFieldHTML='<INPUT TYPE="text" NAME="'+FieldName+'" VALUE="'+FieldValue+'" SIZE="10" MAXLENGTH="10" onChange="MCheckDate(this,\''+LangID+'\')" STYLE="width:'+FieldWidth+'px; height:22px">';
		document.write(formFieldHTML);
	}
}
