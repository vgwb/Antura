---
title: Marsylianka (fr_11) - Script
hide:
---

# Marsylianka (fr_11) - Script
[Quest Index](./index.pl.md) - Language: [english](./fr_11-script.md) - [french](./fr_11-script.fr.md) - polish - [italian](./fr_11-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-11-la-marseillaise/30/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=849141304#gid=849141304)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_11%20Music%20Marseillese/FR_11%20Music%20Marseillese%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code"><code><span class="yarn-header-dim">// fr_11 | La Marseillaise</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-line">Witaj w muzycznej wyprawie! <span class="yarn-meta">#line:0e2f565 </span></span>
<span class="yarn-comment">// &lt;&lt;activity order_marseillese_audio marseillese_played&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetnie! Odkrywałeś „Marsyliankę”. <span class="yarn-meta">#line:06e7b8f </span></span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_music&gt;&gt;</span>
<span class="yarn-line">Hymn reprezentujący Francję i jej naród. <span class="yarn-meta">#line:0c2ef1b </span></span>
<span class="yarn-line">Muzyka może opowiedzieć wiele o historii. <span class="yarn-meta">#line:0f62ef6 </span></span>
<span class="yarn-cmd">&lt;&lt;card musical_score&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Czy możesz zapisać tekst „Marsylianki” w swoim zeszycie? <span class="yarn-meta">#line:00ee9c6 </span></span>
<span class="yarn-line">Narysuj 7 nut: Do Re Mi Fa Sol La Si. <span class="yarn-meta">#line:078a4a9 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-band-member"></a>
## band_member

<div class="yarn-node" data-title="band_member"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 7&gt;&gt;
<span class="yarn-line">Dziękujemy za pomoc! <span class="yarn-meta">#line:09fc7ad </span></span>
<span class="yarn-line">Czy potrafisz to złożyć do kupy? <span class="yarn-meta">#line:0515a95 </span></span>
<span class="yarn-cmd">&lt;&lt;action order_notes&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Cześć! Jesteśmy częścią zespołu. Jesteśmy muzykami. <span class="yarn-meta">#line:09a50f8 </span></span>
<span class="yarn-line">Chcieliśmy zagrać hymn Francji, ale nie możemy! <span class="yarn-meta">#line:0a9df26 </span></span>
<span class="yarn-line">Antura pomyliła scenariusz musicalu. <span class="yarn-meta">#line:0f0ccdf </span></span>
<span class="yarn-line">Znajdź notatki do scenariusza. <span class="yarn-meta">#line:0c9616b </span></span>
<span class="yarn-cmd">&lt;&lt;task_start find_the_script_parts&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-robot"></a>
## npc_robot

<div class="yarn-node" data-title="npc_robot"><pre class="yarn-code"><code><span class="yarn-header-dim">// TESTING</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">teraz zagraj w Jigsaw <span class="yarn-meta">#line:09d7993 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_marseillese marseillese_played&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-ll"></a>
## npc_ll

<div class="yarn-node" data-title="npc_ll"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">teraz odtwórz tę piosenkę <span class="yarn-meta">#line:00f2510 </span></span>
<span class="yarn-cmd">&lt;&lt;activity order_marseillese_audio marseillese_played&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-marseillese-played"></a>
## marseillese_played

<div class="yarn-node" data-title="marseillese_played"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">Brawo! Zagrałeś „Marsyliankę”! <span class="yarn-meta">#line:0a5f3e1</span></span>

</code></pre></div>

<a id="ys-node-item-marseillaise-1"></a>
## item_marseillaise_1

<div class="yarn-node" data-title="item_marseillaise_1"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_1&gt;&gt;</span>
<span class="yarn-line">Napisane jest „Allons enfants” <span class="yarn-meta">#line:0637a75 </span></span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-marseillaise-2"></a>
## item_marseillaise_2

<div class="yarn-node" data-title="item_marseillaise_2"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_2&gt;&gt;</span>
<span class="yarn-line">Napisane jest „De la patrie” <span class="yarn-meta">#line:0265e2d </span></span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-marseillaise-3"></a>
## item_marseillaise_3

<div class="yarn-node" data-title="item_marseillaise_3"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_3&gt;&gt;</span>
<span class="yarn-line">Jest napisane: „Le jour de la gloire” <span class="yarn-meta">#line:0d4d50b </span></span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-marseillaise-4"></a>
## item_marseillaise_4

<div class="yarn-node" data-title="item_marseillaise_4"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_4&gt;&gt;</span>
<span class="yarn-line">Napisano „Est arrivé” <span class="yarn-meta">#line:06d83a3 </span></span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_4&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-note-do"></a>
## item_note_do

<div class="yarn-node" data-title="item_note_do"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_do&gt;&gt;</span>
<span class="yarn-line">„DO” jest pierwszą nutą. <span class="yarn-meta">#line:05d7453 </span></span>

</code></pre></div>

<a id="ys-node-item-note-re"></a>
## item_note_re

<div class="yarn-node" data-title="item_note_re"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_re&gt;&gt;</span>
<span class="yarn-line">„RE” jest drugą nutą. <span class="yarn-meta">#line:027b4cf </span></span>

</code></pre></div>

<a id="ys-node-item-note-mi"></a>
## item_note_mi

<div class="yarn-node" data-title="item_note_mi"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_mi&gt;&gt;</span>
<span class="yarn-line">„MI” jest trzecią nutą. <span class="yarn-meta">#line:05fb9da </span></span>

</code></pre></div>

<a id="ys-node-item-note-fa"></a>
## item_note_fa

<div class="yarn-node" data-title="item_note_fa"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_fa&gt;&gt;</span>
<span class="yarn-line">„FA” jest czwartą nutą. <span class="yarn-meta">#line:06c6c61 </span></span>

</code></pre></div>

<a id="ys-node-item-note-sol"></a>
## item_note_sol

<div class="yarn-node" data-title="item_note_sol"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_sol&gt;&gt;</span>
<span class="yarn-line">„SOL” jest piątą nutą. <span class="yarn-meta">#line:0624b86 </span></span>

</code></pre></div>

<a id="ys-node-item-note-la"></a>
## item_note_la

<div class="yarn-node" data-title="item_note_la"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_la&gt;&gt;</span>
<span class="yarn-line">„LA” jest szóstą nutą. <span class="yarn-meta">#line:05898e9 </span></span>

</code></pre></div>

<a id="ys-node-item-note-si"></a>
## item_note_si

<div class="yarn-node" data-title="item_note_si"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_si&gt;&gt;</span>
<span class="yarn-line">„SI” jest siódmą nutą. <span class="yarn-meta">#line:0e7f004 </span></span>

</code></pre></div>

<a id="ys-node-facts-notes"></a>
## facts_notes

<div class="yarn-node" data-title="facts_notes"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card musical_score&gt;&gt;</span>
<span class="yarn-line">Oto 7 nut: Do Re Mi Fa Sol La Si. <span class="yarn-meta">#line:057ed8c </span></span>
<span class="yarn-line">Powtarzają się w muzyce bezustannie. <span class="yarn-meta">#line:0a9001d </span></span>
<span class="yarn-line">Piszemy piosenki, korzystając z nut. <span class="yarn-meta">#line:0d7a884 </span></span>

</code></pre></div>

<a id="ys-node-facts-marseillaise"></a>
## facts_marseillaise

<div class="yarn-node" data-title="facts_marseillaise"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_music&gt;&gt;</span>
<span class="yarn-line">Hymn został napisany w 1792 roku. <span class="yarn-meta">#line:0edd950 </span></span>
<span class="yarn-cmd">&lt;&lt;card french_revolution&gt;&gt;</span>
<span class="yarn-line">Stał się symbolem rewolucji francuskiej. <span class="yarn-meta">#line:03e8eec </span></span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_1&gt;&gt;</span>
<span class="yarn-line">W każdej części znajdują się mocne słowa. <span class="yarn-meta">#line:0165048 </span></span>
<span class="yarn-line">Muzyka potrafi jednoczyć ludzi. <span class="yarn-meta">#line:01d4688 </span></span>

</code></pre></div>

<a id="ys-node-jean-michelle-jarre"></a>
## jean_michelle_jarre

<div class="yarn-node" data-title="jean_michelle_jarre"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">tags: actor=MAN </span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 11&gt;&gt;
<span class="yarn-line">Dziękuję! Słowa francuskiego hymnu są ważne. <span class="yarn-meta">#line:025e35b </span></span>
<span class="yarn-line">Francuski hymn narodowy „Marsylianka” reprezentuje Francję i jej naród. <span class="yarn-meta">#line:0e4484d </span></span>
<span class="yarn-line">Czy potrafisz ułożyć słowa we właściwej kolejności? <span class="yarn-meta">#line:07e4ff1 </span></span>
<span class="yarn-cmd">&lt;&lt;activity order_words&gt;&gt;</span>
&lt;&lt;elseif $COLLECTED_ITEMS &gt;= 7&gt;&gt;
<span class="yarn-line">Witam, jestem Jean Michelle Jarre. <span class="yarn-meta">#line:02f7c8b </span></span>
<span class="yarn-line">Jestem francuskim kompozytorem i pomagam zespołowi zagrać „Marsyliankę”. <span class="yarn-meta">#line:0bd77b7 </span></span>
<span class="yarn-line">Znajdź słowa hymnu. <span class="yarn-meta">#line:0e7033c </span></span>
<span class="yarn-line">Zostały rozrzucone przez Anturę. <span class="yarn-meta">#line:0dc84d4 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start find_the_words&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Jestem teraz zajęty, przyjdź do mnie później <span class="yarn-meta">#line:08be987 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-win-order"></a>
## win_order

<div class="yarn-node" data-title="win_order"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">Teraz spróbujmy zagrać tę piosenkę! <span class="yarn-meta">#line:0cef358 </span></span>
<span class="yarn-cmd">&lt;&lt;activity play_piano&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spawned-note-musician"></a>
## spawned_note_musician

<div class="yarn-node" data-title="spawned_note_musician"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: note_teacher</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Do Re Mi jest początkiem skali. <span class="yarn-meta">#line:06bcd7a </span></span>
<span class="yarn-line">Następnie nadchodzą Fa i Sol. <span class="yarn-meta">#line:0203a16 </span></span>
<span class="yarn-line">La i Si kończą 7 nut. <span class="yarn-meta">#line:080491f </span></span>
<span class="yarn-line">Nuty pomagają nam czytać nuty. <span class="yarn-meta">#line:003d7fb </span></span>

</code></pre></div>


