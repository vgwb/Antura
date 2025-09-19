---
title: Sąsiedzi Polski (pl_00) - Script
hide:
---

# Sąsiedzi Polski (pl_00) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-00-the-neighbors-of-poland/31/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1929643794#gid=1929643794)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_00%20Geo%20Poland/PL_00%20Geo%20Poland%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_00 | Poland GEO</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags: Start</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $poland_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $poland_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belarus_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belarus_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ukraine_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ukraine_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $slovakia_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $slovakia_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lithuania_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lithuania_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $czech_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $czech_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action area_small&gt;&gt;</span>

<span class="yarn-line">Witamy w Polsce!</span> <span class="yarn-meta">#line:046db1f </span>
<span class="yarn-line">Jesteśmy w Europie</span> <span class="yarn-meta">#line:002aafd </span>
<span class="yarn-line">Poznajmy inne dzieci z pobliskich krajów.</span> <span class="yarn-meta">#line:0862117 </span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To zadanie zostało ukończone.</span> <span class="yarn-meta">#line:085bc39 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dlaczego nie narysujesz teraz swojej flagi?</span> <span class="yarn-meta">#line:01f830b </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-poland-npc"></a>

## poland_npc

<div class="yarn-node" data-title="poland_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// poland</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">tags: actor=KID_F</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $CURRENT_PROGRESS &gt;= $MAX_PROGRESS&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump poland_npc_win&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $poland_completed == true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję za pomoc!</span> <span class="yarn-meta">#line:0a4a56f </span>
<span class="yarn-line">    Czy możesz pomóc moim pozostałym znajomym?</span> <span class="yarn-meta">#line:097eeb8 </span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_poland"&gt;&gt;</span>
<span class="yarn-line">    Tak, to moja flaga! Dziękuję!</span> <span class="yarn-meta">#line:0c9fa89 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_poland remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_polish_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $poland_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
<span class="yarn-line">    Czy potrafisz znaleźć niemiecką flagę?</span> <span class="yarn-meta">#line:04bd4db </span>
    <span class="yarn-cmd">&lt;&lt;set $poland_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action germany_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action area_bigger&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany  &gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    To nie moja flaga. Moja jest biało-czerwona.</span> <span class="yarn-meta">#line:0f967da </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem z Polski!</span> <span class="yarn-meta">#line:017ad80 </span>
<span class="yarn-line">    Antura narobiła bałaganu i wszystkie flagi zostały pomieszane!</span> <span class="yarn-meta">#line:0b9e31d </span>
<span class="yarn-line">    Moja flaga, polska, jest biało-czerwona.</span> <span class="yarn-meta">#line:08a2f6d </span>
<span class="yarn-line">    Czy możesz mi pomóc?</span> <span class="yarn-meta">#line:0fe57ef </span>
    <span class="yarn-cmd">&lt;&lt;set $poland_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-poland"></a>

## task_poland

<div class="yarn-node" data-title="task_poland">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;card flag_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę Polski.</span> <span class="yarn-meta">#line:09e3b54 </span>
<span class="yarn-line">Jest biało-czerwony.</span> <span class="yarn-meta">#line:0b52ba1 </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_polish_FLAG task_poland&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-poland"></a>

## item_flag_poland

<div class="yarn-node" data-title="item_flag_poland">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, asset=flag_poland</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_poland&gt;&gt;</span>
<span class="yarn-line">Flaga Polski.</span> <span class="yarn-meta">#line:07ca581 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_poland"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_poland add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-poland-npc-win"></a>

## poland_npc_win

<div class="yarn-node" data-title="poland_npc_win">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dobra robota! Udało ci się!</span> <span class="yarn-meta">#line:0ba3c4c </span>
<span class="yarn-cmd">&lt;&lt;card concept_europe_map&gt;&gt;</span>
<span class="yarn-line">Odkryłeś część Europy Środkowej!</span> <span class="yarn-meta">#line:031f72c </span>
<span class="yarn-cmd">&lt;&lt;card concept_europe_map zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-germany-npc"></a>

## germany_npc

<div class="yarn-node" data-title="germany_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// GERMANY</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $germany_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-line">    Dziękuję za pomoc!</span> <span class="yarn-meta">#line:0eaf07d </span>
<span class="yarn-line">    Berlin jest stolicą Niemiec.</span> <span class="yarn-meta">#line:0446f03 </span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_germany"&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! To moja flaga!</span> <span class="yarn-meta">#line:0ba8707</span>
<span class="yarn-line">    Czy możesz pomóc mojemu białoruskiemu przyjacielowi?</span> <span class="yarn-meta">#line:06c463a </span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_GERMAN_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action belarus_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    MĘŻCZYZNA: Nasza flaga ma poziome pasy w kolorze czarnym, czerwonym i żółtym.</span> <span class="yarn-meta">#line:0cd7024 </span>
        <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $germany_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem z Niemiec!</span> <span class="yarn-meta">#line:0068fe1 </span>
<span class="yarn-line">    Słyniemy z zamków, lasów i pociągów!</span> <span class="yarn-meta">#line:04dc97a </span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-germany"></a>

## task_germany

<div class="yarn-node" data-title="task_germany">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Znajdź niemiecką flagę i przynieś ją Niemcowi.</span> <span class="yarn-meta">#line:029ee72 </span>
<span class="yarn-line">Ma poziome paski w kolorze czarnym, czerwonym i żółtym.</span> <span class="yarn-meta">#line:0f95ef2 </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_GERMAN_FLAG task_germany&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-belarus-npc"></a>

## belarus_npc

<div class="yarn-node" data-title="belarus_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// belarus</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $belarus_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-line">    Dziękuję za pomoc!</span> <span class="yarn-meta">#line:0a5c214 </span>
<span class="yarn-line">    Mińsk jest stolicą Białorusi!</span> <span class="yarn-meta">#line:0aecb59 </span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_belarus"&gt;&gt;</span>
<span class="yarn-line">    To moja flaga!</span> <span class="yarn-meta">#line:0c57e40 </span>
<span class="yarn-line">    Dziękuję, czy możesz przekazać mojemu czeskiemu przyjacielowi jego flagę?</span> <span class="yarn-meta">#line:021e1a2 </span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belarus remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_belarusian_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action czech_republic_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">        Moja flaga jest czerwona i zielona z czerwonym wzorem po lewej stronie.</span> <span class="yarn-meta">#line:0653fae </span>
        <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $belarus_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_met = true&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem z Białorusi!</span> <span class="yarn-meta">#line:0ccf58d </span>
<span class="yarn-line">    Mamy las pierwotny, który rośnie od stuleci!</span> <span class="yarn-meta">#line:0c6ac62 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-task-belarus"></a>

## task_belarus

<div class="yarn-node" data-title="task_belarus">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę Białorusi.</span> <span class="yarn-meta">#line:0c00afe </span>
<span class="yarn-line">Jest czerwony i zielony, ze wzorem po lewej stronie!</span> <span class="yarn-meta">#line:05c6081 </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_belarusian_FLAG task_belarus&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-czech-republic-npc"></a>

## czech_republic_npc

<div class="yarn-node" data-title="czech_republic_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// czech_republic</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: czech_republic</span>
<span class="yarn-header-dim">tags: actor=KID_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $czech_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_czech_republic&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Naszą stolicą jest Mińsk!</span> <span class="yarn-meta">#line:08473de </span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_czech_republic"&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! To moja flaga!</span> <span class="yarn-meta">#line:07ba10f </span>
<span class="yarn-line">    Pomóż im znaleźć flagę Litwy i przynieś ją im!</span> <span class="yarn-meta">#line:0a1e0a3 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_czech_republic remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_czech_republic_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $czech_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action lithuania_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Moja flaga jest inna! Jest biało-czerwona z niebieskim trójkątem.</span> <span class="yarn-meta">#line:0000741 </span>
    <span class="yarn-cmd">&lt;&lt;set $czech_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $czech_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $czech_met = true&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem z Czech!</span> <span class="yarn-meta">#line:0ded863 </span>
<span class="yarn-line">    Mamy najwięcej zamków w Europie!</span> <span class="yarn-meta">#line:03dc09c </span>
    <span class="yarn-cmd">&lt;&lt;jump task_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-czech-republic"></a>

## task_czech_republic

<div class="yarn-node" data-title="task_czech_republic">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: czech_republic</span>
<span class="yarn-header-dim">tags: actor=KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę Czech.</span> <span class="yarn-meta">#line:0ff23aa </span>
<span class="yarn-line">Jest biało-czerwony z niebieskim trójkątem.</span> <span class="yarn-meta">#line:03cb303 </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_czech_republic_FLAG task_czech_republic&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-lithuania-npc"></a>

## lithuania_npc

<div class="yarn-node" data-title="lithuania_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// lithuania</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $lithuania_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_lithuania&gt;&gt;</span>
<span class="yarn-line">    Dziękujemy za pomoc!</span> <span class="yarn-meta">#line:06cf019 </span>
<span class="yarn-line">    Bruksela jest stolicą Litwy.</span> <span class="yarn-meta">#line:0acbb04 </span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_lithuania"&gt;&gt;</span>
<span class="yarn-line">        Dziękuję, moja piękna flaga powróciła!</span> <span class="yarn-meta">#line:0d2f54c </span>
<span class="yarn-line">        Czy możesz pomóc mojemu ukraińskiemu przyjacielowi?</span> <span class="yarn-meta">#line:0bcf83b </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_lithuania remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_lithuania_FLAG &gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $lithuania_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action ukraine_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Pamiętajcie, moja flaga jest czerwona, zielona i żółta.</span> <span class="yarn-meta">#line:00af906 </span>
        <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem z Litwy!</span> <span class="yarn-meta">#line:0534a85 </span>
<span class="yarn-line">    Jedną trzecią naszego kraju pokrywają lasy!</span> <span class="yarn-meta">#line:0465c31 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-lithuania"></a>

## task_lithuania

<div class="yarn-node" data-title="task_lithuania">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę Litwy.</span> <span class="yarn-meta">#line:0b88326 </span>
<span class="yarn-line">Jest czerwony, zielony i żółty.</span> <span class="yarn-meta">#line:0754062 </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_lithuania_FLAG task_lithuania&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-lithuania"></a>

## item_flag_lithuania

<div class="yarn-node" data-title="item_flag_lithuania">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_lithuania&gt;&gt;</span>
<span class="yarn-line">Flaga Litwy.</span> <span class="yarn-meta">#line:0942331 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_lithuania"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_lithuania add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ukraine-npc"></a>

## ukraine_npc

<div class="yarn-node" data-title="ukraine_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ukraine</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $ukraine_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_ukraine&gt;&gt;</span>
<span class="yarn-line">    Dziękujemy za pomoc!</span> <span class="yarn-meta">#line:02114ba </span>
<span class="yarn-line">    Naszą stolicą jest Kijów.</span> <span class="yarn-meta">#line:01b1e6d </span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_ukraine"&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! To moja flaga.</span> <span class="yarn-meta">#line:05de5ab </span>
<span class="yarn-line">    Czy możesz pomóc mojemu słowackiemu przyjacielowi?</span> <span class="yarn-meta">#line:0aa87ef </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_ukraine remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_ukraine_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action slovakia_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Nie! Nasza flaga jest niebiesko-żółta.</span> <span class="yarn-meta">#line:0a94866 </span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_met = true&gt;&gt;</span>
<span class="yarn-line">    Moi! Jestem z Ukrainy!</span> <span class="yarn-meta">#line:07ea99a </span>
<span class="yarn-line">    Nazywają nas „Spichlerzem” Europy, ponieważ produkujemy duże ilości zboża!</span> <span class="yarn-meta">#line:0dbd4af </span>
    <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ukraine"></a>

## task_ukraine

<div class="yarn-node" data-title="task_ukraine">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę Ukrainy.</span> <span class="yarn-meta">#line:07c148b </span>
<span class="yarn-line">Jest niebiesko-żółty.</span> <span class="yarn-meta">#line:0b6e8e7 </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_ukraine_FLAG task_ukraine&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-ukraine"></a>

## item_flag_ukraine

<div class="yarn-node" data-title="item_flag_ukraine">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_ukraine&gt;&gt;</span>
<span class="yarn-line">Flaga Ukrainy.</span> <span class="yarn-meta">#line:0805b90 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_ukraine"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_ukraine add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-slovakia"></a>

## npc_slovakia

<div class="yarn-node" data-title="npc_slovakia">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// slovakia</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $slovakia_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_slovakia&gt;&gt;</span>
<span class="yarn-line">    Dziękujemy za pomoc!</span> <span class="yarn-meta">#line:06a6231 </span>
<span class="yarn-line">    Stolicą Słowacji jest Bratysława!</span> <span class="yarn-meta">#line:0891aba </span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_slovakia"&gt;&gt;</span>
<span class="yarn-line">    Dziękuję za przywrócenie mojej flagi!</span> <span class="yarn-meta">#line:0d453e9 </span>
<span class="yarn-line">    Wróć na start i odnieś zwycięstwo!</span> <span class="yarn-meta">#line:04bf6d1 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_slovakia remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_slovakia_FLAG &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $slovakia_completed = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">        Nasza flaga jest biała, czerwona i niebieska, z herbem.</span> <span class="yarn-meta">#line:0af30a1 </span>
        <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $slovakia_met = true&gt;&gt;</span>
<span class="yarn-line">    Jestem ze Słowacji!</span> <span class="yarn-meta">#line:054e1b8 </span>
<span class="yarn-line">    Kiedyś byliśmy krajem należącym do Czech.</span> <span class="yarn-meta">#line:0278f6a </span>
    <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-task-slovakia"></a>

## task_slovakia

<div class="yarn-node" data-title="task_slovakia">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę Słowacji.</span> <span class="yarn-meta">#line:04b6692 </span>
<span class="yarn-line">Jest w kolorze białym, czerwonym i niebieskim, z herbem.</span> <span class="yarn-meta">#line:0866ee1 </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_slovakia_FLAG npc_slovakia&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-russia"></a>

## russia

<div class="yarn-node" data-title="russia">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// Russia</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: russia</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć, jestem Rosja.</span> <span class="yarn-meta">#line:065c41c </span>
<span class="yarn-line">To tylko mała część tego wielkiego kraju.</span> <span class="yarn-meta">#line:0a4bae4 </span>
<span class="yarn-cmd">&lt;&lt;card flag_russia&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-belarus"></a>

## item_flag_belarus

<div class="yarn-node" data-title="item_flag_belarus">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-line">Flaga Białorusi</span> <span class="yarn-meta">#line:006ce10 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_belarus"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belarus add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-germany"></a>

## item_flag_germany

<div class="yarn-node" data-title="item_flag_germany">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-line">Flaga Niemiec.</span> <span class="yarn-meta">#line:05ff51a </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_germany"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-czech-republic"></a>

## item_flag_czech_republic

<div class="yarn-node" data-title="item_flag_czech_republic">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: czech_republic</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_czech_republic&gt;&gt;</span>
<span class="yarn-line">Flaga Republiki Czeskiej.</span> <span class="yarn-meta">#line:0fdc68b </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_czech_republic"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_czech_republic add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-slovakia"></a>

## item_flag_slovakia

<div class="yarn-node" data-title="item_flag_slovakia">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_slovakia&gt;&gt;</span>
<span class="yarn-line">Flaga Słowacji.</span> <span class="yarn-meta">#line:0768ab7 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_slovakia"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_slovakia add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>


