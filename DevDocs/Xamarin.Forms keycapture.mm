<map version="freeplane 1.6.0">
<!--To view this file, download free mind mapping software Freeplane from http://freeplane.sourceforge.net -->
<node TEXT="Xamarin.Forms keycapture" FOLDED="false" ID="ID_1834029228" CREATED="1520422053599" MODIFIED="1520422073351" STYLE="oval">
<font SIZE="18"/>
<hook NAME="MapStyle" zoom="1.5">
    <properties fit_to_viewport="false" edgeColorConfiguration="#808080ff,#ff0000ff,#0000ffff,#00ff00ff,#ff00ffff,#00ffffff,#7c0000ff,#00007cff,#007c00ff,#7c007cff,#007c7cff,#7c7c00ff"/>

<map_styles>
<stylenode LOCALIZED_TEXT="styles.root_node" STYLE="oval" UNIFORM_SHAPE="true" VGAP_QUANTITY="24.0 pt">
<font SIZE="24"/>
<stylenode LOCALIZED_TEXT="styles.predefined" POSITION="right" STYLE="bubble">
<stylenode LOCALIZED_TEXT="default" ICON_SIZE="12.0 pt" COLOR="#000000" STYLE="fork">
<font NAME="SansSerif" SIZE="10" BOLD="false" ITALIC="false"/>
</stylenode>
<stylenode LOCALIZED_TEXT="defaultstyle.details"/>
<stylenode LOCALIZED_TEXT="defaultstyle.attributes">
<font SIZE="9"/>
</stylenode>
<stylenode LOCALIZED_TEXT="defaultstyle.note" COLOR="#000000" BACKGROUND_COLOR="#ffffff" TEXT_ALIGN="LEFT"/>
<stylenode LOCALIZED_TEXT="defaultstyle.floating">
<edge STYLE="hide_edge"/>
<cloud COLOR="#f0f0f0" SHAPE="ROUND_RECT"/>
</stylenode>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.user-defined" POSITION="right" STYLE="bubble">
<stylenode LOCALIZED_TEXT="styles.topic" COLOR="#18898b" STYLE="fork">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.subtopic" COLOR="#cc3300" STYLE="fork">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.subsubtopic" COLOR="#669900">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.important">
<icon BUILTIN="yes"/>
</stylenode>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.AutomaticLayout" POSITION="right" STYLE="bubble">
<stylenode LOCALIZED_TEXT="AutomaticLayout.level.root" COLOR="#000000" STYLE="oval" SHAPE_HORIZONTAL_MARGIN="10.0 pt" SHAPE_VERTICAL_MARGIN="10.0 pt">
<font SIZE="18"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,1" COLOR="#0033ff">
<font SIZE="16"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,2" COLOR="#00b439">
<font SIZE="14"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,3" COLOR="#990000">
<font SIZE="12"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,4" COLOR="#111111">
<font SIZE="10"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,5"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,6"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,7"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,8"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,9"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,10"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,11"/>
</stylenode>
</stylenode>
</map_styles>
</hook>
<hook NAME="AutomaticEdgeColor" COUNTER="5" RULE="ON_BRANCH_CREATION"/>
<node TEXT="iOS" POSITION="right" ID="ID_1421205245" CREATED="1520422074307" MODIFIED="1520422078448">
<edge COLOR="#ff0000"/>
<node TEXT="Uses KeyCommand array on focused element" ID="ID_1356847221" CREATED="1520422079526" MODIFIED="1520422122939">
<node TEXT="Only text edit elements and pages appear to be focusable." ID="ID_1354009744" CREATED="1520422127580" MODIFIED="1520422159239">
<node TEXT="Doesn&apos;t seem to work on anything but Pages and TextEntry elements" ID="ID_132201747" CREATED="1520422533075" MODIFIED="1520422548648"/>
</node>
<node TEXT="If KeyCommand is matched, key stroke does not go to UIView" ID="ID_942638223" CREATED="1520422456650" MODIFIED="1520422568735">
<node TEXT="Could there be a work around for this?" ID="ID_170617701" CREATED="1520422578001" MODIFIED="1520422588194">
<font BOLD="true" ITALIC="true"/>
</node>
</node>
<node TEXT="KeyCommand specifies:" ID="ID_622307722" CREATED="1520422791371" MODIFIED="1520422814433">
<node TEXT="input: (string)" ID="ID_1054033004" CREATED="1520422814795" MODIFIED="1520422926950">
<node TEXT="input strings for special keys:" ID="ID_508219256" CREATED="1520423000037" MODIFIED="1520423009846">
<node TEXT="UIKeyInputUpArrow" ID="ID_462344849" CREATED="1520423016729" MODIFIED="1520423024691"/>
<node TEXT="UIKeyInputDownArrow" ID="ID_1276531469" CREATED="1520423025378" MODIFIED="1520423029270"/>
<node TEXT="UIKeyInputLeftArrow" ID="ID_554696738" CREATED="1520423031645" MODIFIED="1520423035817"/>
<node TEXT="UIKeyInputRightArrow" ID="ID_1821620881" CREATED="1520423036270" MODIFIED="1520423039952"/>
<node TEXT="UIKeyInputEscape" ID="ID_1624089668" CREATED="1520423042236" MODIFIED="1520423046099"/>
</node>
</node>
<node TEXT="modifierFlags: (UIKeyModifierFlags)" ID="ID_285760222" CREATED="1520422887695" MODIFIED="1520422937276">
<node TEXT="alphaShift" ID="ID_149256943" CREATED="1520422961558" MODIFIED="1520422969508"/>
<node TEXT="shift" ID="ID_1125483437" CREATED="1520422970996" MODIFIED="1520422972387"/>
<node TEXT="control" ID="ID_1581255926" CREATED="1520422973875" MODIFIED="1520422975033"/>
<node TEXT="alternate" ID="ID_327966855" CREATED="1520422976706" MODIFIED="1520422979394"/>
<node TEXT="command" ID="ID_786820039" CREATED="1520422980893" MODIFIED="1520422982050"/>
<node TEXT="numericPad" ID="ID_1938894128" CREATED="1520422982346" MODIFIED="1520422986172"/>
</node>
<node TEXT="Action: (selector)" ID="ID_1691954119" CREATED="1520422897320" MODIFIED="1520422904666">
<node TEXT="Selector can be mapped to a method." ID="ID_1454763107" CREATED="1520423089217" MODIFIED="1520423104761"/>
<node TEXT="UIKeyCommand triggered is passed to the method" ID="ID_760082086" CREATED="1520423105324" MODIFIED="1520423138157"/>
</node>
<node TEXT="DiscoverabilityTitle: (string)" ID="ID_1453114636" CREATED="1520422908879" MODIFIED="1520422942706"/>
</node>
<node TEXT="Selector" ID="ID_1822940353" CREATED="1520423319558" MODIFIED="1520423330914">
<node TEXT="Passed UIKeyCommand that has been triggered" ID="ID_986002328" CREATED="1520423335518" MODIFIED="1520423346953">
<node TEXT=".Input" ID="ID_1378193685" CREATED="1520423348096" MODIFIED="1520423357383"/>
<node TEXT=".ModifierFlags" ID="ID_987613482" CREATED="1520423358492" MODIFIED="1520423362742"/>
<node TEXT=".DiscoverabilityTitle" ID="ID_270095482" CREATED="1520423381049" MODIFIED="1520423385347"/>
</node>
</node>
</node>
</node>
<node TEXT="Android" POSITION="right" ID="ID_1405709502" CREATED="1520422595512" MODIFIED="1520422597628">
<edge COLOR="#00ff00"/>
<node TEXT="Only approach that works as advertised is using OnKeyUp / OnKeyDown in MainActivity." ID="ID_294137827" CREATED="1520422601581" MODIFIED="1520422699713">
<node TEXT="OnKeyUp" ID="ID_745692961" CREATED="1520423296526" MODIFIED="1520423300558">
<node TEXT="KeyCode keyCode" ID="ID_1889933693" CREATED="1520423397167" MODIFIED="1520423418246">
<node TEXT="enumeration of keyboard keys" ID="ID_1367587822" CREATED="1520423419449" MODIFIED="1520423441123"/>
<node TEXT="Not generally useful unless interpreted by KeyCharacterMap" ID="ID_79465186" CREATED="1520423525599" MODIFIED="1520423565182" LINK="https://developer.xamarin.com/api/type/Android.Views.KeyCharacterMap/"/>
</node>
<node TEXT="KeyEvent keyEvent" FOLDED="true" ID="ID_558847899" CREATED="1520423454555" MODIFIED="1520423458134">
<node TEXT="KeyEventActions Action" FOLDED="true" ID="ID_841899444" CREATED="1520423795811" MODIFIED="1520423950721">
<node TEXT="Down" ID="ID_845079427" CREATED="1520423818232" MODIFIED="1520423821269"/>
<node TEXT="Multiple" ID="ID_78402738" CREATED="1520423821741" MODIFIED="1520423825542"/>
<node TEXT="Up" ID="ID_35716702" CREATED="1520423826092" MODIFIED="1520423826999"/>
</node>
<node TEXT="string Characters" ID="ID_1713838401" CREATED="1520423917431" MODIFIED="1520423936948">
<node TEXT="string of current sequence" ID="ID_50770047" CREATED="1520423919981" MODIFIED="1520423970195"/>
</node>
<node TEXT="char DisplayLabel" ID="ID_206745609" CREATED="1520424004481" MODIFIED="1520424012013">
<node TEXT="Primary character of this key" ID="ID_1029198599" CREATED="1520424014923" MODIFIED="1520424020668"/>
</node>
<node TEXT="Int64 DownTime" ID="ID_1935515448" CREATED="1520424026171" MODIFIED="1520424033798">
<node TEXT=" time of the most recent key down event" ID="ID_1455028374" CREATED="1520424034705" MODIFIED="1520424064450"/>
</node>
<node TEXT="Int64 EventTime" ID="ID_1187944196" CREATED="1520424071433" MODIFIED="1520424076707">
<node TEXT="time of when this event occurred" ID="ID_22310245" CREATED="1520424077756" MODIFIED="1520424084512"/>
</node>
<node TEXT="KeyEventFlags Flags" ID="ID_1424320620" CREATED="1520424094642" MODIFIED="1520424098923">
<node TEXT="Cancelled" ID="ID_868779319" CREATED="1520468354627" MODIFIED="1520468357442">
<node ID="ID_1674863545" CREATED="1520468443041" MODIFIED="1520468443041"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">When associated with up key events, this indicates that the key press has been canceled. Typically this is used with virtual touch screen keys, where the user can slide from the virtual key area on to the display: in that case, the application will receive a canceled up event and should not perform the action normally associated with the key. Note that for this to work, the application can not perform an action for a key until it receives an up or the long press timeout has expired.</font></span>
  </body>
</html>
</richcontent>
</node>
</node>
<node TEXT="CancelledLongPress" ID="ID_440310701" CREATED="1520468367096" MODIFIED="1520468373307">
<node ID="ID_1069166332" CREATED="1520468451685" MODIFIED="1520468451685" LINK="https://developer.xamarin.com/api/field/Android.Views.KeyEventFlags.Canceled/"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">Set when a key event has</font></span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px"><span>&#160;</span></font><tt style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: 400; font-style: normal; font-family: monospace; color: rgb(78, 87, 88); font-size: 14px; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font face="Consolas, monospace" color="rgb(52, 152, 219)" size="0.95em"><a href="https://developer.xamarin.com/api/field/Android.Views.KeyEventFlags.Canceled/" class="cref" style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: normal; font-style: normal; font-family: Consolas, monospace; color: rgb(52, 152, 219); text-decoration: none; font-size: 0.95em">KeyEventFlags.Canceled</a></font></tt><span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">&#160;</font></span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px"><span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none">set because a long press action was executed while it was down.</span></font>
  </body>
</html>
</richcontent>
</node>
</node>
<node TEXT="EditorAction" ID="ID_1294227538" CREATED="1520468550030" MODIFIED="1520468552813">
<node ID="ID_751002562" CREATED="1520468562068" MODIFIED="1520468562068"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">This mask is used for compatibility, to identify enter keys that are coming from an IME whose enter key has been auto-labelled &quot;next&quot; or &quot;done&quot;. This allows TextView to dispatch these as normal enter keys for old applications, but still do the appropriate action when receiving them.</font></span>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="Fallback" ID="ID_1227288664" CREATED="1520468382648" MODIFIED="1520468384225">
<node ID="ID_400651348" CREATED="1520468595210" MODIFIED="1520468595210"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">Set when a key event has been synthesized to implement default behavior for an event that the application did not handle. Fallback key events are generated by unhandled trackball motions (to emulate a directional keypad) and by certain unhandled key presses that are declared in the key map (such as special function numeric keypad keys when numlock is off).</font></span>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="FromSystem" ID="ID_224454746" CREATED="1520468621007" MODIFIED="1520468623066">
<node ID="ID_160992194" CREATED="1520468624151" MODIFIED="1520468624151"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">This mask is set if an event was known to come from a trusted part of the system. That is, the event is known to come from the user, and could not have been spoofed by a third party component</font></span>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="KeepTouchMode" ID="ID_545205843" CREATED="1520468388104" MODIFIED="1520468390945">
<node ID="ID_247663463" CREATED="1520468635118" MODIFIED="1520468635118"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">This mask is set if we don't want the key event to cause us to leave touch mode.</font></span>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="LongPress" ID="ID_1842945761" CREATED="1520468357888" MODIFIED="1520468359722">
<node ID="ID_1367082279" CREATED="1520468644732" MODIFIED="1520468644732"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">This flag is set for the first key repeat that occurs after the long press timeout.</font></span>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="Softkeyboard" ID="ID_1070706339" CREATED="1520468395688" MODIFIED="1520468398569">
<node ID="ID_1970989168" CREATED="1520468655756" MODIFIED="1520468655756"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">This mask is set if the key event was generated by a software keyboard.</font></span>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="Tracking" ID="ID_706800931" CREATED="1520468400752" MODIFIED="1520468402466">
<node ID="ID_414115579" CREATED="1520468663381" MODIFIED="1520468663381" LINK="https://developer.xamarin.com/api/field/Android.Views.KeyEventActions.Up/"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">Set for</font></span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px"><span>&#160;</span></font><tt style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: 400; font-style: normal; font-family: monospace; color: rgb(78, 87, 88); font-size: 14px; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font face="Consolas, monospace" color="rgb(52, 152, 219)" size="0.95em"><a href="https://developer.xamarin.com/api/field/Android.Views.KeyEventActions.Up/" class="cref" style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: normal; font-style: normal; font-family: Consolas, monospace; color: rgb(52, 152, 219); text-decoration: none; font-size: 0.95em">KeyEventActions.Up</a></font></tt><span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">&#160;</font></span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px"><span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none">when this event's key code is still being tracked from its initial down. That is, somebody requested that tracking started on the key down and a long press has not caused the tracking to be canceled.</span></font>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="VirtualHardKey" ID="ID_370340709" CREATED="1520468404792" MODIFIED="1520468412633">
<node ID="ID_1865371389" CREATED="1520468688324" MODIFIED="1520468688324"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">This key event was generated by a virtual (on-screen) hard key area. Typically this is an area of the touchscreen, outside of the regular display, dedicated to &quot;hardware&quot; buttons</font></span>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="WokeHere" ID="ID_802368646" CREATED="1520468415047" MODIFIED="1520468416969">
<node ID="ID_233366772" CREATED="1520468704901" MODIFIED="1520468704901"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">This mask is set if the device woke because of this key event</font></span>
  </body>
</html>

</richcontent>
</node>
</node>
</node>
<node TEXT="bool IsAltPressed" ID="ID_747517020" CREATED="1520468747070" MODIFIED="1520468754385"/>
<node TEXT="bool IsCancelled" ID="ID_32911918" CREATED="1520468758662" MODIFIED="1520468761681">
<node ID="ID_1852831502" CREATED="1520468774672" MODIFIED="1520468774672" LINK="https://developer.xamarin.com/api/type/Android.Views.KeyEvent/!:Android.Views.KeyEvent.ACTION_UP"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">For</font></span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px"><span>&#160;</span></font><tt style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: 400; font-style: normal; font-family: monospace; color: rgb(78, 87, 88); font-size: 14px; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font face="inherit" color="rgb(52, 152, 219)" size="14px"><a href="https://developer.xamarin.com/api/type/Android.Views.KeyEvent/!:Android.Views.KeyEvent.ACTION_UP" style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: normal; font-style: normal; font-family: inherit; color: rgb(52, 152, 219)">KeyEvent.ACTION_UP</a></font></tt><span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">&#160;</font></span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px"><span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none">events, indicates that the event has been canceled as per</span><span>&#160;</span></font><tt style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: 400; font-style: normal; font-family: monospace; color: rgb(78, 87, 88); font-size: 14px; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font face="inherit" color="rgb(95, 174, 227)" size="14px"><a href="https://developer.xamarin.com/api/type/Android.Views.KeyEvent/!:Android.Views.KeyEvent.FLAG_CANCELED" style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: normal; font-style: normal; font-family: inherit; color: rgb(95, 174, 227)">KeyEvent.FLAG_CANCELED</a></font></tt>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="IsCapsLockOn" ID="ID_261203942" CREATED="1520468776350" MODIFIED="1520468781840"/>
<node TEXT="IsCntrlPressed" ID="ID_421627061" CREATED="1520468784262" MODIFIED="1520468790688"/>
<node TEXT="IsFunctionPressed" ID="ID_153966848" CREATED="1520468794454" MODIFIED="1520468798976"/>
<node TEXT="IsLongPress" ID="ID_924862010" CREATED="1520468802182" MODIFIED="1520468804263"/>
<node TEXT="IsMetaPressed" ID="ID_1576190987" CREATED="1520468806318" MODIFIED="1520468809816"/>
<node TEXT="IsNumLockOn" ID="ID_1980058357" CREATED="1520468820925" MODIFIED="1520468823784"/>
<node TEXT="IsPrintingKey" ID="ID_1624998958" CREATED="1520468825774" MODIFIED="1520468829255"/>
<node TEXT="IsScrollLockOn" ID="ID_671674963" CREATED="1520468830998" MODIFIED="1520468835656"/>
<node TEXT="IsShiftKeyPressed" ID="ID_1184008182" CREATED="1520468837734" MODIFIED="1520468841647"/>
<node TEXT="IsSymKeyPressed" ID="ID_205788730" CREATED="1520468843790" MODIFIED="1520468850455"/>
<node TEXT="IsSystem" ID="ID_452065737" CREATED="1520468853230" MODIFIED="1520468863224">
<node ID="ID_440524996" CREATED="1520468872010" MODIFIED="1520468872010"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">Is this a system key? System keys can not be used for menu shortcuts</font></span>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="IsTracking" ID="ID_172696327" CREATED="1520468873189" MODIFIED="1520468876743">
<node ID="ID_224243135" CREATED="1520468882622" MODIFIED="1520468882622" LINK="https://developer.xamarin.com/api/type/Android.Views.KeyEvent/!:Android.Views.KeyEvent.ACTION_UP"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">&#160;</font></span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px"><span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none">For</span><span>&#160;</span></font><tt style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: 400; font-style: normal; font-family: monospace; color: rgb(78, 87, 88); font-size: 14px; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font face="inherit" color="rgb(52, 152, 219)" size="14px"><a href="https://developer.xamarin.com/api/type/Android.Views.KeyEvent/!:Android.Views.KeyEvent.ACTION_UP" style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: normal; font-style: normal; font-family: inherit; color: rgb(52, 152, 219)">KeyEvent.ACTION_UP</a></font></tt><span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">&#160;</font></span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px"><span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none">events, indicates that the event is still being tracked from its initial down event as per</span><span>&#160;</span></font><tt style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: 400; font-style: normal; font-family: monospace; color: rgb(78, 87, 88); font-size: 14px; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font face="inherit" color="rgb(52, 152, 219)" size="14px"><a href="https://developer.xamarin.com/api/type/Android.Views.KeyEvent/!:Android.Views.KeyEvent.FLAG_TRACKING" style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: normal; font-style: normal; font-family: inherit; color: rgb(52, 152, 219)">KeyEvent.FLAG_TRACKING</a></font></tt>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="KeyCharacterMap" ID="ID_22367271" CREATED="1520468889549" MODIFIED="1520468894080">
<node ID="ID_380500842" CREATED="1520468900806" MODIFIED="1520468900806" LINK="https://developer.xamarin.com/api/type/Android.Views.KeyCharacterMap/"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">Gets the</font></span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px"><span>&#160;</span></font><tt style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: 400; font-style: normal; font-family: monospace; color: rgb(78, 87, 88); font-size: 14px; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255)"><font face="Consolas, monospace" color="rgb(52, 152, 219)" size="0.95em"><a href="https://developer.xamarin.com/api/type/Android.Views.KeyCharacterMap/" class="cref" style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: normal; font-style: normal; font-family: Consolas, monospace; color: rgb(52, 152, 219); text-decoration: none; font-size: 0.95em">KeyCharacterMap</a></font></tt><span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">&#160;</font></span><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px"><span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none">associated with the keyboard device</span></font>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="KeyCode" ID="ID_267351780" CREATED="1520468912085" MODIFIED="1520468913479"/>
<node TEXT="char Number" ID="ID_1298895593" CREATED="1520468934453" MODIFIED="1520468946543">
<node ID="ID_1869388125" CREATED="1520468947814" MODIFIED="1520468947814" LINK="https://developer.xamarin.com/api/type/System.Char/"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <font color="rgb(52, 152, 219)" face="Consolas, monospace" size="0.95em"><i style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255)"><a href="https://developer.xamarin.com/api/type/System.Char/" class="cref" style="margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; border-top-style: none; border-top-width: 0px; border-right-style: none; border-right-width: 0px; border-bottom-style: none; border-bottom-width: 0px; border-left-style: none; border-left-width: 0px; font-weight: normal; font-style: normal; font-family: Consolas, monospace; color: rgb(52, 152, 219); text-decoration: none; font-size: 0.95em">Char</a></i></font><span style="color: rgb(78, 87, 88); font-family: WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none"><font color="rgb(78, 87, 88)" face="WeblySleek UI, Segoe UI, Helvetica Neue, Arial, sans-serif" size="14px">. Gets the number or symbol associated with the key.</font></span>
  </body>
</html>

</richcontent>
</node>
</node>
<node TEXT="Int32 RepeatCount" ID="ID_1154872721" CREATED="1520468953678" MODIFIED="1520468958456"/>
<node TEXT="Int32 ScanCode" ID="ID_1725774596" CREATED="1520468966557" MODIFIED="1520468970958"/>
<node TEXT="InputSourceType Source" ID="ID_1523955613" CREATED="1520468976398" MODIFIED="1520468987167">
<node TEXT="one of which is Keyboard" ID="ID_1070935554" CREATED="1520469031893" MODIFIED="1520469038359"/>
</node>
<node TEXT="Int32 UnicodeChar" ID="ID_1846707378" CREATED="1520469142723" MODIFIED="1520469147598"/>
</node>
</node>
</node>
</node>
<node TEXT="UWP" POSITION="right" ID="ID_825526717" CREATED="1520469170867" MODIFIED="1520469172894">
<edge COLOR="#ff00ff"/>
<node TEXT="Only approach that works all the time is the combined use of the following two Window.Current.CoreWindow events:" ID="ID_1846589677" CREATED="1520469285444" MODIFIED="1520469721419">
<node ID="ID_355971121" CREATED="1520469764486" MODIFIED="1520469764486"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span class="hljs-keyword" style="color: rgb(1, 1, 253); font-family: Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: pre-wrap; word-spacing: 0px"><font color="rgb(1, 1, 253)" face="Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif" size="14px">public</font></span><span style="color: rgb(0, 0, 0); font-family: Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: pre-wrap; word-spacing: 0px; background-color: rgb(249, 249, 249); display: inline !important; float: none"><font color="rgb(0, 0, 0)" face="Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif" size="14px"> </font></span><span class="hljs-keyword" style="color: rgb(1, 1, 253); font-family: Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: pre-wrap; word-spacing: 0px"><font color="rgb(1, 1, 253)" face="Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif" size="14px">event</font></span><span style="color: rgb(0, 0, 0); font-family: Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: pre-wrap; word-spacing: 0px; background-color: rgb(249, 249, 249); display: inline !important; float: none"><font color="rgb(0, 0, 0)" face="Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif" size="14px"> TypedEventHandler CharacterReceived&lt;CoreWindow, CharacterReceivedEventArgs&gt;</font></span>
  </body>
</html>

</richcontent>
</node>
<node ID="ID_1983989422" CREATED="1520469736874" MODIFIED="1520469736874"><richcontent TYPE="NODE">

<html>
  <head>
    
  </head>
  <body>
    <span class="hljs-keyword" style="color: rgb(1, 1, 253); font-family: Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: pre-wrap; word-spacing: 0px"><font color="rgb(1, 1, 253)" face="Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif" size="14px">public</font></span><span style="color: rgb(0, 0, 0); font-family: Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: pre-wrap; word-spacing: 0px; background-color: rgb(249, 249, 249); display: inline !important; float: none"><font color="rgb(0, 0, 0)" face="Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif" size="14px"> </font></span><span class="hljs-keyword" style="color: rgb(1, 1, 253); font-family: Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: pre-wrap; word-spacing: 0px"><font color="rgb(1, 1, 253)" face="Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif" size="14px">event</font></span><span style="color: rgb(0, 0, 0); font-family: Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif; font-size: 14px; font-style: normal; font-weight: 400; letter-spacing: normal; text-align: left; text-indent: 0px; text-transform: none; white-space: pre-wrap; word-spacing: 0px; background-color: rgb(249, 249, 249); display: inline !important; float: none"><font color="rgb(0, 0, 0)" face="Consolas, Menlo, Monaco, Lucida Console, Liberation Mono, DejaVu Sans Mono, Bitstream Vera Sans Mono, Courier New, monospace, sans-serif" size="14px"> TypedEventHandler KeyUp&lt;CoreWindow, KeyEventArgs&gt;</font></span>
  </body>
</html>

</richcontent>
</node>
<node TEXT="NOTE: Apps do not receive these events when an IME is enabled. The Input Method Editor (IME) handles all keyboard input and sets Handled to true." ID="ID_1982176736" CREATED="1520469470347" MODIFIED="1520469480805"/>
</node>
</node>
<node TEXT="Concept:" POSITION="right" ID="ID_645539608" CREATED="1520471760638" MODIFIED="1520471764904">
<edge COLOR="#00ffff"/>
<node TEXT="Forms9Patch.KeyCommandListener" ID="ID_672952809" CREATED="1520471768557" MODIFIED="1520472898345">
<node TEXT="public" ID="ID_1603688726" CREATED="1520472738152" MODIFIED="1520472739650">
<node TEXT="Input" ID="ID_1855290441" CREATED="1520471793181" MODIFIED="1520471800887"/>
<node TEXT="ModifierFlags" ID="ID_245319915" CREATED="1520471833085" MODIFIED="1520471838271"/>
<node TEXT="Command" ID="ID_529958593" CREATED="1520471853981" MODIFIED="1520471865271"/>
<node TEXT="Command Params" ID="ID_333809558" CREATED="1520471866037" MODIFIED="1520471870863"/>
<node TEXT="Entered" ID="ID_208086393" CREATED="1520471872013" MODIFIED="1520471885895"/>
<node TEXT="DiscoverabilityTitle" ID="ID_118367220" CREATED="1520471890581" MODIFIED="1520471894623"/>
</node>
</node>
<node TEXT="KeyCommandEventArgs" ID="ID_282998850" CREATED="1520472951887" MODIFIED="1520475019937">
<node TEXT="KeyCommand" ID="ID_1197066110" CREATED="1520472960527" MODIFIED="1520472963330"/>
<node TEXT="Element" ID="ID_1501409978" CREATED="1520472964551" MODIFIED="1520472966465"/>
</node>
<node TEXT="VisualElement extensions" ID="ID_828677281" CREATED="1520471914861" MODIFIED="1520472724290">
<node TEXT="public static void AddKeyCommandListener();" ID="ID_2530056" CREATED="1520472211307" MODIFIED="1520472871825"/>
<node TEXT="public List&lt;KeyCommand&gt; KeyCommands" ID="ID_628322775" CREATED="1520472873080" MODIFIED="1520472888657"/>
</node>
</node>
</node>
</map>
