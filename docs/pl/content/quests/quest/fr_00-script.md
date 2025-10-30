---
title: Sąsiedzi Francji (fr_00) - Script
hide:
---

# Sąsiedzi Francji (fr_00) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-00-the-neighbors-of-france/22)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1044148815#gid=1044148815)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_00%20Geo%20France/FR_00%20Geo%20France%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_00 | France GEO</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags: Start</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $COUNTRIES_COMPLETED = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $TOTAL_COUNTRIES = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $france_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $france_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $italy_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $italy_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $swiss_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $swiss_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lux_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lux_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belgium_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belgium_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $spain_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $spain_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_france&gt;&gt;</span>

<span class="yarn-line">Witamy we Francji!</span> <span class="yarn-meta">#line:046db1f </span>
<span class="yarn-line">Jesteśmy w Europie.</span> <span class="yarn-meta">#line:08a8ce8 </span>
<span class="yarn-line">Poznajmy dzieci z pobliskich krajów.</span> <span class="yarn-meta">#line:08a09de </span>
<span class="yarn-cmd">&lt;&lt;target NPC_France&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Zadanie ukończone.</span> <span class="yarn-meta">#line:0432689 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Czy możesz teraz narysować swoją flagę?</span> <span class="yarn-meta">#line:0c48a14 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-win"></a>

## win

<div class="yarn-node" data-title="win">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// FRANCE</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dobra robota! Udało ci się!</span> <span class="yarn-meta">#line:0ba3c4c </span>
<span class="yarn-cmd">&lt;&lt;card concept_europe_map&gt;&gt;</span>
<span class="yarn-line">Znalazłeś część Europy!</span> <span class="yarn-meta">#line:06e1dd4 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-france-npc"></a>

## france_npc

<div class="yarn-node" data-title="france_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COUNTRIES_COMPLETED &gt;= $TOTAL_COUNTRIES&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump win&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $france_completed&gt;&gt;</span>
<span class="yarn-line">    Dziękuję za pomoc!</span> <span class="yarn-meta">#line:04e2d8b </span>
<span class="yarn-line">    Czy możesz pomóc moim pozostałym znajomym?</span> <span class="yarn-meta">#line:0e426ba </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_france")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_france"&gt;&gt;</span>
<span class="yarn-line">        Tak, to moja flaga! Dziękuję!</span> <span class="yarn-meta">#line:01e24a8 </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_france remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_FRENCH_FLAG&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $france_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
<span class="yarn-line">        Czy potrafisz znaleźć niemiecką flagę?</span> <span class="yarn-meta">#line:04bd4db </span>
        <span class="yarn-cmd">&lt;&lt;set $france_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action germany_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_GE&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;area area_europe&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_germany  &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    To nie moja flaga. Moja jest niebiesko-biało-czerwona.</span> <span class="yarn-meta">#line:04e0432 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $france_met = true&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem z Francji!</span> <span class="yarn-meta">#line:06dfdd6 </span>
<span class="yarn-line">    Antura pomyliła wszystkie flagi!</span> <span class="yarn-meta">#line:0a20eed </span>
<span class="yarn-line">    Moja flaga jest niebiesko-biało-czerwona.</span> <span class="yarn-meta">#line:0868737 </span>
<span class="yarn-line">    Czy możesz mi pomóc?</span> <span class="yarn-meta">#line:0a9f34e </span>
    <span class="yarn-cmd">&lt;&lt;jump task_france&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-france"></a>

## task_france

<div class="yarn-node" data-title="task_france">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset flag_france&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę francuską.</span> <span class="yarn-meta">#line:0e35434 #task:FIND_FRENCH_FLAG</span>
<span class="yarn-cmd">&lt;&lt;target Flag_France&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_FR&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_FRENCH_FLAG task_france_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-france-done"></a>

## task_france_done

<div class="yarn-node" data-title="task_france_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_france&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_france add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_France&gt;&gt;</span>
[MISSING TRANSLATION: Great! You found the French flag. Go back to the French kid!]

</code>
</pre>
</div>

<a id="ys-node-item-flag-france"></a>

## item_flag_france

<div class="yarn-node" data-title="item_flag_france">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_france&gt;&gt;</span>
<span class="yarn-line">Flaga Francji.</span> <span class="yarn-meta">#line:01d9617 </span>
<span class="yarn-cmd">&lt;&lt;inventory flag_france add&gt;&gt;</span>

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
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $germany_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-line">    Dziękuję za pomoc!</span> <span class="yarn-meta">#line:0eaf07d </span>
<span class="yarn-line">    Berlin jest stolicą Niemiec.</span> <span class="yarn-meta">#line:0446f03 </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_germany")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_germany"&gt;&gt;</span>
<span class="yarn-line">        Dziękuję! To moja flaga!</span> <span class="yarn-meta">#line:0ba8707</span>
<span class="yarn-line">        Czy możesz pomóc mojemu hiszpańskiemu przyjacielowi?</span> <span class="yarn-meta">#line:03684cb </span>
        <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_germany remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_GERMAN_FLAG &gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action spain_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $germany_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_SP&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_spain&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        [MISSING TRANSLATION:         You have my flag in your inventory!]</span> <span class="yarn-meta">#line:0ffc06f </span>
<span class="yarn-line">        [MISSING TRANSLATION:         It's black, red and yellow! Select it and talk to me again!]</span> <span class="yarn-meta">#line:07be3df </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Nasza flaga ma paski: czarny, czerwony, żółty.</span> <span class="yarn-meta">#line:0cd7024 </span>
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
<span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-line">Znajdź niemiecką flagę i przynieś ją.</span> <span class="yarn-meta">#line:029ee72 #task:FIND_GERMAN_FLAG</span>
<span class="yarn-line">Posiada paski: czarny, czerwony, żółty.</span> <span class="yarn-meta">#line:0f95ef2 </span>
<span class="yarn-line">[MISSING TRANSLATION: &lt;camera_focus camera_Flag_GE&gt;&gt;]</span> <span class="yarn-meta">#line:0b35bca </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_GERMAN_FLAG task_germany&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spain-npc"></a>

## spain_npc

<div class="yarn-node" data-title="spain_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// SPAIN</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: spain</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $spain_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_spain&gt;&gt;</span>
<span class="yarn-line">    Dziękuję za pomoc!</span> <span class="yarn-meta">#line:0a5c214 </span>
<span class="yarn-line">    Barcelona i Madryt to duże miasta w Hiszpanii.</span> <span class="yarn-meta">#line:09cf6c9 </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_spain")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_spain"&gt;&gt;</span>
<span class="yarn-line">        To moja flaga!</span> <span class="yarn-meta">#line:0c57e40 </span>
<span class="yarn-line">        Dziękuję. Czy możesz przekazać mojemu włoskiemu przyjacielowi ich flagę?</span> <span class="yarn-meta">#line:0930602 </span>
        <span class="yarn-cmd">&lt;&lt;set $spain_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_spain remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_SPANISH_FLAG &gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action italy_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_IT&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $spain_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_italy&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        [MISSING TRANSLATION:         You have my flag in your inventory!]</span> <span class="yarn-meta">#line:0c7aab5 </span>
<span class="yarn-line">        [MISSING TRANSLATION:         It's the red and yellow one, select it!]</span> <span class="yarn-meta">#line:05049ea </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Nie moja! Nasza flaga jest czerwono-żółta.</span> <span class="yarn-meta">#line:0db05da </span>
    <span class="yarn-cmd">&lt;&lt;jump task_spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $spain_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $spain_met = true&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem z Hiszpanii!</span> <span class="yarn-meta">#line:0f5bc06 </span>
<span class="yarn-line">    Mamy taniec, który nazywa się flamenco!</span> <span class="yarn-meta">#line:05e6d48 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-task-spain"></a>

## task_spain

<div class="yarn-node" data-title="task_spain">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: spain</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_spain&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę Hiszpanii.</span> <span class="yarn-meta">#line:091cc7c #task:FIND_SPANISH_FLAG</span>
<span class="yarn-line">Jest czerwony i żółty jak słońce.</span> <span class="yarn-meta">#line:09635b4</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_SP&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_SPANISH_FLAG task_spain&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-italy-npc"></a>

## italy_npc

<div class="yarn-node" data-title="italy_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ITALY</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $italy_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>
<span class="yarn-line">    Dziękujemy! Naszą stolicą jest Rzym!</span> <span class="yarn-meta">#line:0148edb</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_italy")&gt;&gt;</span> 
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_italy"&gt;&gt;</span>
<span class="yarn-line">        Dziękuję! To moja flaga!</span> <span class="yarn-meta">#line:081ac66 </span>
<span class="yarn-line">        Pomóż im znaleźć belgijską flagę!</span> <span class="yarn-meta">#line:001f54f </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_italy remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_ITALIAN_FLAG &gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $italy_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action belgium_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_BE&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_belgium&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        [MISSING TRANSLATION:         You have my flag in your inventory!]</span> <span class="yarn-meta">#line:0a7e4bb </span>
<span class="yarn-line">        [MISSING TRANSLATION:         It's red, white and green, select it and talk to mea again!]</span> <span class="yarn-meta">#line:0d7ae3f </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Moja flaga jest zielono-biało-czerwona.</span> <span class="yarn-meta">#line:0dc8623</span>
    <span class="yarn-cmd">&lt;&lt;set $italy_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $italy_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $italy_met = true&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem z Włoch!</span> <span class="yarn-meta">#line:0bda0fc </span>
    <span class="yarn-cmd">&lt;&lt;jump task_italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-italy"></a>

## task_italy

<div class="yarn-node" data-title="task_italy">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>
<span class="yarn-line">Znajdź włoską flagę.</span> <span class="yarn-meta">#line:0ed29f1 #task:FIND_ITALIAN_FLAG</span>
<span class="yarn-line">Jest zielony, biały i czerwony jak pizza!</span> <span class="yarn-meta">#line:0cde44c </span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_Italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_ITALIAN_FLAG task_italy&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-belgium-npc"></a>

## belgium_npc

<div class="yarn-node" data-title="belgium_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// BELGIUM</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: belgium</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $belgium_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_belgium&gt;&gt;</span>
<span class="yarn-line">    Dziękujemy za pomoc!</span> <span class="yarn-meta">#line:080a099 </span>
<span class="yarn-line">    Bruksela jest stolicą Belgii.</span> <span class="yarn-meta">#line:06b3ceb </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_belgium") &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_belgium"&gt;&gt;</span>
<span class="yarn-line">        Dziękuję! Moja flaga wróciła!</span> <span class="yarn-meta">#line:079096a </span>
<span class="yarn-line">        Czy możesz pomóc mojemu przyjacielowi z Luksemburga?</span> <span class="yarn-meta">#line:03a9b7d </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_belgium remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_BELGIUM_FLAG &gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $belgium_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action lux_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_LU&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $belgium_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_lux&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else &gt;&gt;</span>
<span class="yarn-line">        [MISSING TRANSLATION:         You have my flag in your inventory!]</span> <span class="yarn-meta">#line:0326f38 </span>
<span class="yarn-line">        [MISSING TRANSLATION:         Select it to give it to me by clicking on it.]</span> <span class="yarn-meta">#line:049b7a1 </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Pamiętaj: moja flaga ma paski: czarny, żółty, czerwony.</span> <span class="yarn-meta">#line:0141a13 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_belgium&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belgium_met = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belgium_met = true&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem z Belgii!</span> <span class="yarn-meta">#line:0a61b67 </span>
<span class="yarn-line">    Mówimy również po francusku!</span> <span class="yarn-meta">#line:0b18893 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_belgium&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-belgium"></a>

## task_belgium

<div class="yarn-node" data-title="task_belgium">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: belgium</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belgium&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę belgijską.</span> <span class="yarn-meta">#line:0f08126 #task:FIND_BELGIUM_FLAG</span>
<span class="yarn-line">Jest czarny, żółty i czerwony z pionowymi paskami.</span> <span class="yarn-meta">#line:079ecca </span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_BE&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_BELGIUM_FLAG task_belgium&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-belgium"></a>

## item_flag_belgium

<div class="yarn-node" data-title="item_flag_belgium">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: belgium</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belgium&gt;&gt;</span>
<span class="yarn-line">Flaga Belgii.</span> <span class="yarn-meta">#line:0b11066 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_belgium"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belgium add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-luxembourg-npc"></a>

## luxembourg_npc

<div class="yarn-node" data-title="luxembourg_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// LUXEMBURG</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: lux</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $lux_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_luxembourg&gt;&gt;</span>
<span class="yarn-line">    Dziękujemy za pomoc!</span> <span class="yarn-meta">#line:02114ba </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_luxembourg")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_luxembourg"&gt;&gt;</span>
<span class="yarn-line">        Dziękuję! To moja flaga.</span> <span class="yarn-meta">#line:05de5ab </span>
<span class="yarn-line">        Czy możesz pomóc mojemu szwajcarskiemu przyjacielowi?</span> <span class="yarn-meta">#line:0090192 </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_luxembourg remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_LUX_FLAG&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $lux_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action swiss_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_SW&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_swiss&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        [MISSING TRANSLATION:         You have my flag in your inventory!]</span> <span class="yarn-meta">#line:061ab8b </span>
<span class="yarn-line">        [MISSING TRANSLATION:         It's the red white and light blue one! Select it and give it to me.]</span> <span class="yarn-meta">#line:0bbe7e1 </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Nie! Nasza flaga jest czerwono-biało-jasnoniebieska.</span> <span class="yarn-meta">#line:0529472 </span>
    <span class="yarn-cmd">&lt;&lt;set $lux_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_lux&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lux_met = true&gt;&gt;</span>
<span class="yarn-line">    Moien! Jestem z Luksemburga!</span> <span class="yarn-meta">#line:0b22d58 </span>
<span class="yarn-line">    Jesteśmy małą firmą i mówimy trzema językami!</span> <span class="yarn-meta">#line:0a3d0e3 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_lux&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-lux"></a>

## task_lux

<div class="yarn-node" data-title="task_lux">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: lux</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_luxembourg&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę Luksemburga.</span> <span class="yarn-meta">#line:0ed3698 #task:FIND_LUX_FLAG</span>
<span class="yarn-line">Jest w kolorze czerwonym, białym i jasnoniebieskim.</span> <span class="yarn-meta">#line:018e4cf </span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_LU&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_LUX_FLAG task_lux&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-lux"></a>

## item_flag_lux

<div class="yarn-node" data-title="item_flag_lux">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: lux</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_luxembourg&gt;&gt;</span>
<span class="yarn-line">Flaga Luksemburga.</span> <span class="yarn-meta">#line:05badd7 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_luxembourg"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_luxembourg add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-swiss"></a>

## npc_swiss

<div class="yarn-node" data-title="npc_swiss">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// SWISS</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: swiss</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $swiss_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_switzerland&gt;&gt;</span>
<span class="yarn-line">    Dziękujemy za pomoc!</span> <span class="yarn-meta">#line:02f73c8 </span>
<span class="yarn-line">    Stolicą Szwajcarii jest Berno!</span> <span class="yarn-meta">#line:0d2e2d7 </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_switzerland")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_switzerland"&gt;&gt;</span>
<span class="yarn-line">        Dziękuję za przywrócenie mojej flagi!</span> <span class="yarn-meta">#line:0ca99a0 </span>
<span class="yarn-line">        Wróć na start i odbierz swoją nagrodę!</span> <span class="yarn-meta">#line:0148420 </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_switzerland remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_SWISS_FLAG&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $swiss_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus NPC_France&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        [MISSING TRANSLATION:         You have my flag in your inventory!]</span> <span class="yarn-meta">#line:0f2651c </span>
<span class="yarn-line">        [MISSING TRANSLATION:         Select it and give it to me! It's the red one with the cross.]</span> <span class="yarn-meta">#line:060512c </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    To nie moja flaga. Nasza flaga jest czerwona z białym krzyżem.</span> <span class="yarn-meta">#line:0caad5a </span>
    <span class="yarn-cmd">&lt;&lt;jump task_swiss&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $swiss_met = true&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem ze Szwajcarii!</span> <span class="yarn-meta">#line:09fe8fc </span>
<span class="yarn-line">    Słyniemy z ośnieżonych gór i sera.</span> <span class="yarn-meta">#line:0bf6b0d </span>
    <span class="yarn-cmd">&lt;&lt;jump task_swiss&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-task-swiss"></a>

## task_swiss

<div class="yarn-node" data-title="task_swiss">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: swiss</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_switzerland&gt;&gt;</span>
<span class="yarn-line">Znajdź flagę Szwajcarii.</span> <span class="yarn-meta">#line:0ec7096 #task:FIND_SWISS_FLAG</span>
<span class="yarn-line">Jest czerwony i ma duży biały krzyż, wygląda jak apteczka pierwszej pomocy.</span> <span class="yarn-meta">#line:07cc57e </span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_SW&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_SWISS_FLAG npc_swiss&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-monaco"></a>

## monaco

<div class="yarn-node" data-title="monaco">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// MONACO</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: monaco</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć! Jestem z Monako!</span> <span class="yarn-meta">#line:0a3f8f6 </span>
<span class="yarn-line">Jesteśmy mali. Mamy wyścigi samochodowe i pałac nad morzem!</span> <span class="yarn-meta">#line:0dc315a </span>
<span class="yarn-cmd">&lt;&lt;asset flag_monaco&gt;&gt;</span>
<span class="yarn-line">Moja flaga jest czerwono-biała.</span> <span class="yarn-meta">#line:00bd939 </span>

</code>
</pre>
</div>

<a id="ys-node-andorra"></a>

## andorra

<div class="yarn-node" data-title="andorra">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ANDORRA</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: andorra</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć! Jestem z Andory!</span> <span class="yarn-meta">#line:071f85c </span>
<span class="yarn-line">Moja flaga jest niebiesko-żółto-czerwona.</span> <span class="yarn-meta">#line:02846e8 </span>
<span class="yarn-cmd">&lt;&lt;asset flag_andorra&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-spain"></a>

## item_flag_spain

<div class="yarn-node" data-title="item_flag_spain">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">group: spain</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_spain&gt;&gt;</span>
<span class="yarn-line">Flaga Hiszpanii</span> <span class="yarn-meta">#line:006ce10 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_spain"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_spain add&gt;&gt;</span>
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
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-line">Flaga Niemiec.</span> <span class="yarn-meta">#line:05ff51a </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_germany"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-italy"></a>

## item_flag_italy

<div class="yarn-node" data-title="item_flag_italy">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>
<span class="yarn-line">Flaga Włoch.</span> <span class="yarn-meta">#line:0fdc68b </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_italy"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_italy add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-switzerland"></a>

## item_flag_switzerland

<div class="yarn-node" data-title="item_flag_switzerland">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: swiss</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_switzerland&gt;&gt;</span>
<span class="yarn-line">Flaga Szwajcarii.</span> <span class="yarn-meta">#line:0768ab7 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_switzerland"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_switzerland add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-npc"></a>

## spawned_npc

<div class="yarn-node" data-title="spawned_npc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: all </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">W Europie ludzie mówią ponad 200 językami!</span> <span class="yarn-meta">#line:04f4280 </span>
<span class="yarn-line">Do wielu z tych krajów możesz dotrzeć bez paszportu!</span> <span class="yarn-meta">#line:08d624d </span>
<span class="yarn-line">Francja jest największym krajem w Unii Europejskiej.</span> <span class="yarn-meta">#line:0ab3dd9 </span>

</code>
</pre>
</div>


