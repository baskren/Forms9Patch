<map version="freeplane 1.3.0">
<!--To view this file, download free mind mapping software Freeplane from http://freeplane.sourceforge.net -->
<node TEXT="Forms9Patch.List&#xa;data schema" ID="ID_1723255651" CREATED="1283093380553" MODIFIED="1478184523217"><hook NAME="MapStyle">

<map_styles>
<stylenode LOCALIZED_TEXT="styles.root_node">
<stylenode LOCALIZED_TEXT="styles.predefined" POSITION="right">
<stylenode LOCALIZED_TEXT="default" MAX_WIDTH="600" COLOR="#000000" STYLE="as_parent">
<font NAME="SansSerif" SIZE="10" BOLD="false" ITALIC="false"/>
</stylenode>
<stylenode LOCALIZED_TEXT="defaultstyle.details"/>
<stylenode LOCALIZED_TEXT="defaultstyle.note"/>
<stylenode LOCALIZED_TEXT="defaultstyle.floating">
<edge STYLE="hide_edge"/>
<cloud COLOR="#f0f0f0" SHAPE="ROUND_RECT"/>
</stylenode>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.user-defined" POSITION="right">
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
<stylenode LOCALIZED_TEXT="styles.AutomaticLayout" POSITION="right">
<stylenode LOCALIZED_TEXT="AutomaticLayout.level.root" COLOR="#000000">
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
</stylenode>
</stylenode>
</map_styles>
</hook>
<hook NAME="AutomaticEdgeColor" COUNTER="7"/>
<node TEXT="Forms9Patch.DataTemplateSelector" POSITION="right" ID="ID_806226294" CREATED="1478180020419" MODIFIED="1478182858341">
<edge COLOR="#ff0000"/>
<node TEXT="a dictionary that matches data object types to view types" ID="ID_1491540665" CREATED="1478180471348" MODIFIED="1478182858339"/>
<node TEXT="Xamarin.Forms.ListView will pass it an object and expects, in return, a DataTemplate." ID="ID_1636040921" CREATED="1478180547242" MODIFIED="1478182856298"/>
<node TEXT="the DataTemplate returned is for CellView&lt;T&gt; where T is the view type that matches the object type given in the request." ID="ID_6971099" CREATED="1478180722128" MODIFIED="1478180951309"/>
</node>
<node TEXT="Forms9Patch.CellView&lt;TView&gt;" POSITION="right" ID="ID_261361793" CREATED="1478180043988" MODIFIED="1478182851648">
<edge COLOR="#0000ff"/>
<arrowlink SHAPE="CUBIC_CURVE" COLOR="#000000" WIDTH="2" TRANSPARENCY="80" FONT_SIZE="9" FONT_FAMILY="SansSerif" DESTINATION="ID_6971099" STARTINCLINATION="9;-139;" ENDINCLINATION="-175;0;" STARTARROW="NONE" ENDARROW="DEFAULT"/>
<node TEXT="A Xamarin.Forms.ViewCell that contains the custom view" ID="ID_1403497785" CREATED="1478181728009" MODIFIED="1478181818782"/>
<node TEXT="object BindingContext" ID="ID_96113793" CREATED="1478180051623" MODIFIED="1478180342800">
<node TEXT="really of type IItem" ID="ID_1728697505" CREATED="1478181031851" MODIFIED="1478181067575"/>
<node TEXT="when changed, View.BindingContext is set to BindingContext, so corresponding IItem is bound to View" ID="ID_1905280281" CREATED="1478181068061" MODIFIED="1478181622127"/>
</node>
<node TEXT="TView View" ID="ID_476040918" CREATED="1478180074274" MODIFIED="1478182433976">
<node TEXT="Is set to BaseCellView (a Forms9Patch.BaseCell), used to provide latent functionality and contain the view for type T" ID="ID_793633502" CREATED="1478181957692" MODIFIED="1478182222315"/>
<node TEXT="T is the view type matched to the object type in the request for a cell DataTemplate from Xamarin.Forms.ListView" ID="ID_745009008" CREATED="1478180956550" MODIFIED="1478180989535"/>
<node TEXT="CellView&lt;T&gt; instantiation instantiates a view of type T and sets it to BaseCellView.View" ID="ID_1412006697" CREATED="1478180990159" MODIFIED="1478182199278"/>
</node>
<node TEXT="Height" ID="ID_1711994861" CREATED="1478181185022" MODIFIED="1478181186614">
<node TEXT="can be set if ListView.HasUnevenRows==true;" ID="ID_1547778276" CREATED="1478181187246" MODIFIED="1478181224604"/>
</node>
</node>
<node TEXT="Forms9Patch.BaseCellView" POSITION="right" ID="ID_1894260354" CREATED="1478181985982" MODIFIED="1478183063539">
<edge COLOR="#ffff00"/>
<arrowlink SHAPE="CUBIC_CURVE" COLOR="#000000" WIDTH="2" TRANSPARENCY="80" FONT_SIZE="9" FONT_FAMILY="SansSerif" DESTINATION="ID_476040918" STARTINCLINATION="13;-70;" ENDINCLINATION="-131;0;" STARTARROW="NONE" ENDARROW="DEFAULT"/>
<node TEXT="Content" ID="ID_276448820" CREATED="1478181991508" MODIFIED="1478183231951">
<node TEXT="Actual view developer wanted in the cell" ID="ID_875294105" CREATED="1478181993394" MODIFIED="1478182020649"/>
<node TEXT="Could have ICellView interface" ID="ID_128680763" CREATED="1478183184650" MODIFIED="1478183202130"/>
</node>
<node TEXT="BindingContext" ID="ID_119437136" CREATED="1478184382355" MODIFIED="1478184385459">
<node TEXT="When changed, it sets Content.BindingContext to Item.Source. Is set to an Item, set by CellView&lt;TView&gt;(when its BindingContext is changed)." ID="ID_1042283444" CREATED="1478184385848" MODIFIED="1478184489973"/>
</node>
</node>
<node TEXT="Forms9Patch.Item&lt;Tobj&gt;" POSITION="right" ID="ID_1808855495" CREATED="1478180292961" MODIFIED="1478184523217">
<edge COLOR="#ff00ff"/>
<arrowlink SHAPE="CUBIC_CURVE" COLOR="#000000" WIDTH="2" TRANSPARENCY="80" FONT_SIZE="9" FONT_FAMILY="SansSerif" DESTINATION="ID_119437136" STARTINCLINATION="-8;-32;" ENDINCLINATION="-212;-12;" STARTARROW="NONE" ENDARROW="DEFAULT"/>
<node TEXT="Manages state information and latent functionality model for the cell" ID="ID_213234091" CREATED="1478181684439" MODIFIED="1478181717843"/>
<node TEXT="T Source" ID="ID_781480298" CREATED="1478180053427" MODIFIED="1478180338457">
<node TEXT="the actual data object" ID="ID_149639797" CREATED="1478181149600" MODIFIED="1478181158016"/>
</node>
<node TEXT="BindingContext" ID="ID_1795729431" CREATED="1478181626406" MODIFIED="1478181628454"/>
<node TEXT="HasUnevenRows" ID="ID_1287665428" CREATED="1478183343026" MODIFIED="1478183347388">
<node TEXT="bound to parentGroup" ID="ID_1836571964" CREATED="1478183348349" MODIFIED="1478183364797"/>
</node>
</node>
<node TEXT="Forms9Patch.Group" POSITION="right" ID="ID_330674876" CREATED="1478182916956" MODIFIED="1478182921669">
<edge COLOR="#7c0000"/>
<node TEXT="A list of Forms9Patch.Item&lt;Tobj&gt;, a proxy for items source that enables additional functionality (visibility, accessories, drag &amp; drop)" ID="ID_1310089811" CREATED="1478182922371" MODIFIED="1478183030759"/>
<node TEXT="HasUnevenRows" ID="ID_184488086" CREATED="1478183366709" MODIFIED="1478183373773">
<node TEXT="either bound to parent Group or Forms9Patch.ListView" ID="ID_1688119231" CREATED="1478183374164" MODIFIED="1478183720355"/>
</node>
</node>
<node TEXT="TView" POSITION="right" ID="ID_632550284" CREATED="1478181466700" MODIFIED="1478183070580">
<edge COLOR="#00ffff"/>
<arrowlink SHAPE="CUBIC_CURVE" COLOR="#000000" WIDTH="2" TRANSPARENCY="80" FONT_SIZE="9" FONT_FAMILY="SansSerif" DESTINATION="ID_276448820" STARTINCLINATION="3;-55;" ENDINCLINATION="-238;-127;" STARTARROW="NONE" ENDARROW="DEFAULT"/>
<node TEXT="BindingContext" ID="ID_527299812" CREATED="1478181474552" MODIFIED="1478181477594">
<node TEXT="set to the data object by Item&lt;T&gt; when it&apos;s BindingContext is changed" ID="ID_1823739416" CREATED="1478181478016" MODIFIED="1478181507860"/>
</node>
<node TEXT="ICellView.RowHeight" ID="ID_1020740219" CREATED="1478182342168" MODIFIED="1478182355803">
<node TEXT="used to set the RowHeight is ListView.HasUnevenRows==true;" ID="ID_859634949" CREATED="1478182344499" MODIFIED="1478182368938"/>
</node>
</node>
</node>
</map>
