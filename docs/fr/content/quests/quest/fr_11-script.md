---
title: La Marseillaise (fr_11) - Script
hide:
---

# La Marseillaise (fr_11) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-11-la-marseillaise/30/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=849141304#gid=849141304)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_11%20Music%20Marseillese/FR_11%20Music%20Marseillese%20-%20Yarn%20Script.yarn)  

<div class="yarn-node">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// fr_11 | La Marseillaise</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// </span>
</code>
</pre>
</div>

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $quest1_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $activity1_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $quest2_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $activity2_done = false&gt;&gt;</span>
<span class="yarn-line">Bienvenue dans la quête musicale !</span> <span class="yarn-meta">#line:0e2f565 </span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_music zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-band-member"></a>

## band_member

<div class="yarn-node" data-title="band_member">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $quest1_done == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $activity1_done == false&gt;&gt;</span>
<span class="yarn-line">        Merci de nous aider !</span> <span class="yarn-meta">#line:09fc7ad</span>
        <span class="yarn-cmd">&lt;&lt;card musical_score notes&gt;&gt;</span>
<span class="yarn-line">        Peux-tu le mettre ensemble ?</span> <span class="yarn-meta">#line:0515a95</span>
        <span class="yarn-cmd">&lt;&lt;set $activity1_done = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;activity order_Musical_notes&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;elseif $activity1_done ==true &gt;&gt;</span>
<span class="yarn-line">        [MISSING TRANSLATION:         We're gonna play soon!]</span> <span class="yarn-meta">#line:0ef876c </span>
<span class="yarn-line">        [MISSING TRANSLATION:         Do you wanna try to order them again?]</span> <span class="yarn-meta">#line:08ae1b5 </span>
<span class="yarn-line">        [MISSING TRANSLATION:         -&gt; Yes]</span> <span class="yarn-meta">#line:0a1e74f </span>
        <span class="yarn-cmd">&lt;&lt;activity order_Musical_notes band_member&gt;&gt;</span>
<span class="yarn-line">        [MISSING TRANSLATION:         -&gt; No]</span> <span class="yarn-meta">#line:05d9168 </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Bonjour ! Nous faisons partie d'un groupe. Nous sommes musiciens.</span> <span class="yarn-meta">#line:09a50f8 </span>
<span class="yarn-line">Nous voulions jouer l'hymne français, mais nous ne pouvons pas !</span> <span class="yarn-meta">#line:0a9df26 </span>
<span class="yarn-line">Antura a mélangé le scénario musical.</span> <span class="yarn-meta">#line:0f0ccdf </span>
<span class="yarn-line">Retrouvez les notes du script.</span> <span class="yarn-meta">#line:0c9616b #task:find_the_script_parts</span>
<span class="yarn-cmd">&lt;&lt;task_start find_the_script_parts win_quest1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-win-quest1"></a>

## win_quest1

<div class="yarn-node" data-title="win_quest1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $quest1_done = true&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: You found the musical notes!]</span> <span class="yarn-meta">#line:0c3d5b7 </span>
<span class="yarn-line">[MISSING TRANSLATION: Do, the first one.]</span> <span class="yarn-meta">#line:090aa20 #card:note_do</span>
<span class="yarn-line">[MISSING TRANSLATION: Re, the second one.]</span> <span class="yarn-meta">#line:01ef658 #card:note_re</span>
<span class="yarn-line">[MISSING TRANSLATION: Mi, the third one.]</span> <span class="yarn-meta">#line:0c78966 #card:note_mi</span>
<span class="yarn-line">[MISSING TRANSLATION: Fa, the fourth one.]</span> <span class="yarn-meta">#line:0c185a1 #card:note_fa</span>
<span class="yarn-line">[MISSING TRANSLATION: Sol, the fifth one.]</span> <span class="yarn-meta">#line:0338a81 #card:note_sol</span>
<span class="yarn-line">[MISSING TRANSLATION: La, the sixth one.]</span> <span class="yarn-meta">#line:0809b07 #card:note_la</span>
<span class="yarn-line">[MISSING TRANSLATION: Si, the last one and the highest note.]</span> <span class="yarn-meta">#line:04a7724 #card:note_si</span>
<span class="yarn-line">[MISSING TRANSLATION: Good job! Now speak with the band manager.]</span> <span class="yarn-meta">#line:0064edf </span>

</code>
</pre>
</div>

<a id="ys-node-jean-michelle-jarre"></a>

## jean_michelle_jarre

<div class="yarn-node" data-title="jean_michelle_jarre">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $activity2_done == true&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     Great job!]</span> <span class="yarn-meta">#line:0227e33 </span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $quest2_done == true &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $activity2_done = true&gt;&gt;</span>
<span class="yarn-line">    Merci ! Les paroles de l'hymne français sont importantes.</span> <span class="yarn-meta">#line:025e35b </span>
<span class="yarn-line">    L'hymne national français, « La Marseillaise », représente la France et son peuple.</span> <span class="yarn-meta">#line:0e4484d </span>
<span class="yarn-line">    Peux-tu mettre les mots dans l'ordre ?</span> <span class="yarn-meta">#line:07e4ff1 </span>
    <span class="yarn-cmd">&lt;&lt;activity  order_marseillese_audio marseillese_played&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $activity1_done == true &gt;&gt;</span>
<span class="yarn-line">    Bonjour, je m'appelle Jean Michelle Jarre.</span> <span class="yarn-meta">#line:02f7c8b</span>
<span class="yarn-line">    Je suis un compositeur français et j'aide le groupe à jouer "La Marseillaise".</span> <span class="yarn-meta">#line:0bd77b7 </span>
<span class="yarn-line">    Trouvez les paroles de l'hymne.</span> <span class="yarn-meta">#line:0e7033c </span>
<span class="yarn-line">    Ils ont été dispersés par Antura.</span> <span class="yarn-meta">#line:0dc84d4 </span>
    <span class="yarn-cmd">&lt;&lt;task_start find_the_words win_quest2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis occupé en ce moment, viens me parler plus tard</span> <span class="yarn-meta">#line:08be987 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-win-order"></a>

## win_order

<div class="yarn-node" data-title="win_order">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Essayons maintenant de jouer la chanson !</span> <span class="yarn-meta">#line:0cef358 </span>
<span class="yarn-cmd">&lt;&lt;activity play_piano&gt;&gt;</span>

</code>
</pre>
</div>

<div class="yarn-node">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">title : win_quest2</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $quest2_done = true&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: These are the words of la Marseillaise!]</span> <span class="yarn-meta">#line:08ea860 </span>
<span class="yarn-line">[MISSING TRANSLATION: The first part.]</span> <span class="yarn-meta">#line:0f8bb5a </span>
<span class="yarn-cmd">&lt;&lt;card  marseillaise_1 zoom&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: The second part.]</span> <span class="yarn-meta">#line:0f1831e </span>
<span class="yarn-cmd">&lt;&lt;card  marseillaise_2 zoom&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: The third part.]</span> <span class="yarn-meta">#line:018f56a </span>
<span class="yarn-cmd">&lt;&lt;card  marseillaise_3 zoom&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: The last part.]</span> <span class="yarn-meta">#line:050d937 </span>
<span class="yarn-cmd">&lt;&lt;card  marseillaise_4 zoom&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Now go talk with Jean!]</span> <span class="yarn-meta">#line:0a28908 </span>
<span class="yarn-cmd">&lt;&lt;target Jean_MJ&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_end find_the_words&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-npc-robot"></a>

## npc_robot

<div class="yarn-node" data-title="npc_robot">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// TESTING</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Hello!]</span> <span class="yarn-meta">#line:0b8b684 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-ll"></a>

## npc_ll

<div class="yarn-node" data-title="npc_ll">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Good morning!]</span> <span class="yarn-meta">#line:0913236 </span>

</code>
</pre>
</div>

<a id="ys-node-marseillese-played"></a>

## marseillese_played

<div class="yarn-node" data-title="marseillese_played">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bravo ! Vous avez joué « La Marseillaise » !</span> <span class="yarn-meta">#line:0a5f3e1</span>

</code>
</pre>
</div>

<a id="ys-node-item-marseillaise-1"></a>

## item_marseillaise_1

<div class="yarn-node" data-title="item_marseillaise_1">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_1&gt;&gt;</span>
<span class="yarn-line">Il est écrit « Allons les enfants »</span> <span class="yarn-meta">#line:0637a75 </span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-marseillaise-2"></a>

## item_marseillaise_2

<div class="yarn-node" data-title="item_marseillaise_2">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_2&gt;&gt;</span>
<span class="yarn-line">Il est écrit « De la patrie »</span> <span class="yarn-meta">#line:0265e2d </span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-marseillaise-3"></a>

## item_marseillaise_3

<div class="yarn-node" data-title="item_marseillaise_3">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_3&gt;&gt;</span>
<span class="yarn-line">Il est écrit "Le jour de la gloire"</span> <span class="yarn-meta">#line:0d4d50b </span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-marseillaise-4"></a>

## item_marseillaise_4

<div class="yarn-node" data-title="item_marseillaise_4">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_4&gt;&gt;</span>
<span class="yarn-line">Il est écrit "Est arrivé"</span> <span class="yarn-meta">#line:06d83a3 </span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_4&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-note-do"></a>

## item_note_do

<div class="yarn-node" data-title="item_note_do">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_do&gt;&gt;</span>
<span class="yarn-line">"DO" est la première note.</span> <span class="yarn-meta">#line:05d7453 </span>

</code>
</pre>
</div>

<a id="ys-node-item-note-re"></a>

## item_note_re

<div class="yarn-node" data-title="item_note_re">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_re&gt;&gt;</span>
<span class="yarn-line">"RE" est la deuxième note.</span> <span class="yarn-meta">#line:027b4cf </span>

</code>
</pre>
</div>

<a id="ys-node-item-note-mi"></a>

## item_note_mi

<div class="yarn-node" data-title="item_note_mi">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_mi&gt;&gt;</span>
<span class="yarn-line">"MI" est la troisième note.</span> <span class="yarn-meta">#line:05fb9da </span>

</code>
</pre>
</div>

<a id="ys-node-item-note-fa"></a>

## item_note_fa

<div class="yarn-node" data-title="item_note_fa">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_fa&gt;&gt;</span>
<span class="yarn-line">"FA" est la quatrième note.</span> <span class="yarn-meta">#line:06c6c61 </span>

</code>
</pre>
</div>

<a id="ys-node-item-note-sol"></a>

## item_note_sol

<div class="yarn-node" data-title="item_note_sol">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_sol&gt;&gt;</span>
<span class="yarn-line">"SOL" est la cinquième note.</span> <span class="yarn-meta">#line:0624b86 </span>

</code>
</pre>
</div>

<a id="ys-node-item-note-la"></a>

## item_note_la

<div class="yarn-node" data-title="item_note_la">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_la&gt;&gt;</span>
<span class="yarn-line">"LA" est la sixième note.</span> <span class="yarn-meta">#line:05898e9 </span>

</code>
</pre>
</div>

<a id="ys-node-item-note-si"></a>

## item_note_si

<div class="yarn-node" data-title="item_note_si">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card note_si&gt;&gt;</span>
<span class="yarn-line">« SI » est la septième note.</span> <span class="yarn-meta">#line:0e7f004 </span>

</code>
</pre>
</div>

<a id="ys-node-facts-notes"></a>

## facts_notes

<div class="yarn-node" data-title="facts_notes">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card musical_score&gt;&gt;</span>
<span class="yarn-line">Ce sont 7 notes : Do Re Mi Fa Sol La Si.</span> <span class="yarn-meta">#line:057ed8c </span>
<span class="yarn-line">Ils se répètent encore et encore dans la musique.</span> <span class="yarn-meta">#line:0a9001d </span>
<span class="yarn-line">Nous utilisons des notes pour écrire des chansons.</span> <span class="yarn-meta">#line:0d7a884 </span>

</code>
</pre>
</div>

<a id="ys-node-facts-marseillaise"></a>

## facts_marseillaise

<div class="yarn-node" data-title="facts_marseillaise">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_music&gt;&gt;</span>
<span class="yarn-line">L'hymne a été écrit en 1792.</span> <span class="yarn-meta">#line:0edd950 </span>
<span class="yarn-cmd">&lt;&lt;card french_revolution&gt;&gt;</span>
<span class="yarn-line">Il est devenu un symbole pendant la Révolution française.</span> <span class="yarn-meta">#line:03e8eec </span>
<span class="yarn-cmd">&lt;&lt;card marseillaise_1&gt;&gt;</span>
<span class="yarn-line">Chaque partie contient des mots puissants.</span> <span class="yarn-meta">#line:0165048 </span>
<span class="yarn-line">La musique peut unir les gens.</span> <span class="yarn-meta">#line:01d4688 </span>


</code>
</pre>
</div>

<a id="ys-node-secret-ll"></a>

## secret_ll

<div class="yarn-node" data-title="secret_ll">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: You found me!]</span> <span class="yarn-meta">#line:0d732f4 </span>
<span class="yarn-cmd">&lt;&lt;card arc_de_triomphe zoom&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: We're standing on top of the Arc Du Triomphe!]</span> <span class="yarn-meta">#line:0236a86 </span>
<span class="yarn-line">[MISSING TRANSLATION: I'll reward you with some more cookies!]</span> <span class="yarn-meta">#line:0bb0568 </span>
<span class="yarn-cmd">&lt;&lt;action cookies_unleashed&gt;&gt;</span>


</code>
</pre>
</div>


