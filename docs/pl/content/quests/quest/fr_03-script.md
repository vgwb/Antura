---
title: Juliusz Verne i transport (fr_03) - Script
hide:
---

# Juliusz Verne i transport (fr_03) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-03-jules-verne-and-transportation/25/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=336647638#gid=336647638)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_03%20Nantes%20Verne/FR_03%20Nantes%20Verne%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_03 | Jules Verne (Nantes)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - FIND_BOOKS (collect 4 Jules Verne books)</span>
<span class="yarn-header-dim">// - COLLECT_TRAIN (collect from "Around the World in 80 Days")</span>
<span class="yarn-header-dim">// - COLLECT_ROCKET (collect from "From Earth to the Moon")</span>
<span class="yarn-header-dim">// - COLLECT_SUBMARINE (collect from "20,000 Leagues Under the Sea")</span>
<span class="yarn-header-dim">// - COLLECT_BALLOON (collect from "Five Weeks in a Balloon")</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $QUEST_ITEMS = 4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $museum_clean = false&gt;&gt;</span>
<span class="yarn-line">Jesteśmy w NANTES, w MUZEUM Juliusza Verne'a!</span> <span class="yarn-meta">#line:0b5e2f3</span>
<span class="yarn-line">Odkryjmy tego PISARZA i jego niezwykłe PODRÓŻE.</span> <span class="yarn-meta">#line:006e7fc</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_door&gt;&gt;</span>
<span class="yarn-comment">// DEBUG</span>
<span class="yarn-comment">//&lt;&lt;set $museum_clean= true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;SetActive museum_door false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Zadanie ukończone! Gratulacje!</span> <span class="yarn-meta">#line:08f0d37 </span>
<span class="yarn-line">Teraz wiesz o Juliuszu Verne i jego KSIĄŻKACH!</span> <span class="yarn-meta">#line:0174104</span>
<span class="yarn-line">Kiedy przyjedziesz do Nantes, koniecznie odwiedź nasze MUZEUM!</span> <span class="yarn-meta">#line:0f72ec7 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">W ramach zajęć w klasie możesz NARYSOWAĆ ŁÓDŹ PODWODNĄ lub RAKIETĘ!</span> <span class="yarn-meta">#line:087acef </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-door"></a>

## npc_door

<div class="yarn-node" data-title="npc_door">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("clean_sidewalk")&gt;&gt;</span>
<span class="yarn-line">    Dziękuję za posprzątanie!</span> <span class="yarn-meta">#line:0cec96a </span>
<span class="yarn-line">    Muzeum jest już otwarte! Porozmawiaj z PRZEWODNIKIEM PO MUZEUM!</span> <span class="yarn-meta">#line:0d2e2f0 </span>
    <span class="yarn-cmd">&lt;&lt;SetActive museum_door false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target museum_guide&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jaki BAŁAGANOWANY jest teren przed MUZEUM!</span> <span class="yarn-meta">#line:0a3e2f4</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_sidewalk&gt;&gt;</span>
<span class="yarn-line">    Ten niebieski PIES biegł i narobił bałaganu!</span> <span class="yarn-meta">#line:0210ef0 </span>
<span class="yarn-line">    Nie mogę otworzyć MUZEUM, skoro panuje tu taki bałagan.</span> <span class="yarn-meta">#line:0d58543 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">    Pomóż mi posprzątać chodnik!</span> <span class="yarn-meta">#line:01a9d0f </span>
    <span class="yarn-cmd">&lt;&lt;SetActive antura false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive trash_down false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive trash_up true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_full&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start clean_sidewalk task_clean_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-clean-done"></a>

## task_clean_done

<div class="yarn-node" data-title="task_clean_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dobrze! Wróć do portiera.</span> <span class="yarn-meta">#line:0f8dd6c </span>
<span class="yarn-cmd">&lt;&lt;target npc_door&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-museum-guide"></a>

## museum_guide

<div class="yarn-node" data-title="museum_guide">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;area area_full&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if $museum_clean == false&gt;&gt;</span>
<span class="yarn-line">    Mamy PROBLEM!</span> <span class="yarn-meta">#line:0642734 </span>
<span class="yarn-line">    Zabawny niebieski PIES wszedł do MUZEUM i narobił bałaganu!</span> <span class="yarn-meta">#line:0525efc</span>
<span class="yarn-line">    Czy możesz pomóc w sprzątaniu MUZEUM?</span> <span class="yarn-meta">#line:09eafa1</span>
    <span class="yarn-cmd">&lt;&lt;task_start clean_museum task_clean_museum_done&gt;&gt;</span>
&lt;&lt;elseif $COLLECTED_ITEMS &gt;= $QUEST_ITEMS&gt;&gt;
<span class="yarn-line">    Dziękuję! Teraz MUZEUM jest w porządku</span> <span class="yarn-meta">#line:0c81640 </span>
<span class="yarn-line">    Juliusz Verne byłby z ciebie dumny!</span> <span class="yarn-meta">#line:0bc6c7b </span>
<span class="yarn-line">    Proszę dopasować KSIĄŻKI do POJAZDÓW!</span> <span class="yarn-meta">#line:0245e0e </span>
    <span class="yarn-cmd">&lt;&lt;activity match_verne_vehicles activity_match_done&gt;&gt;</span>
&lt;&lt;elseif $COLLECTED_ITEMS &gt;0&gt;&gt;
<span class="yarn-line">    Dobrze ci idzie!</span> <span class="yarn-meta">#line:0c52f3c </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Mamy kolejny PROBLEM!</span> <span class="yarn-meta">#line:0a7dfb3 </span>
<span class="yarn-line">    Nasze pojazdy specjalne zaginęły.</span> <span class="yarn-meta">#line:0bb46da </span>
<span class="yarn-line">    Proszę porozmawiać z opiekunem pierwszego pokoju.</span> <span class="yarn-meta">#line:0386706 </span>
    <span class="yarn-cmd">&lt;&lt;target tutor_20000&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-done"></a>

## activity_match_done

<div class="yarn-node" data-title="activity_match_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Gratulacje! Udało Ci się dopasować wszystkie KSIĄŻKI do POJAZDÓW.</span> <span class="yarn-meta">#line:0e86b01 </span>
<span class="yarn-line">Teraz wszystko jest gotowe na przyjęcie gości!</span> <span class="yarn-meta">#line:00eefe0 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-clean-museum-done"></a>

## task_clean_museum_done

<div class="yarn-node" data-title="task_clean_museum_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Idealnie. Teraz muzeum jest czyste! Wróć do PRZEWODNIKA PO MUZEUM.</span> <span class="yarn-meta">#line:0b5eb24 </span>
<span class="yarn-cmd">&lt;&lt;set $museum_clean = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target museum_guide&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-20000"></a>

## tutor_20000

<div class="yarn-node" data-title="tutor_20000">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// --------------------------------------------------------------------------- </span>
<span class="yarn-header-dim">// 20000 - SUBMARINE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_submarine")&gt;&gt;</span>
<span class="yarn-line">    Znalazłeś ŁÓDŹ PODWODNĄ!</span> <span class="yarn-meta">#line:0666ee2 </span>
<span class="yarn-line">    Ale jest POŁAMANE na kawałki!</span> <span class="yarn-meta">#line:097c3fc </span>
<span class="yarn-line">    Czy możesz to naprawić?</span> <span class="yarn-meta">#line:02e7812</span>
    <span class="yarn-cmd">&lt;&lt;inventory submarine_nautilus remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_verne_submarine activity_submarine_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card submarine_nautilus&gt;&gt;</span>
<span class="yarn-line">    Pomocy! Nautilus zaginął!</span> <span class="yarn-meta">#line:00a6154 </span>
<span class="yarn-line">    Znajdź łódź podwodną</span> <span class="yarn-meta">#line:0bb264b</span>
<span class="yarn-line">    Jeśli nie możesz tego znaleźć, UŻYJ MAPY</span> <span class="yarn-meta">#line:05bc5e9 </span>
    <span class="yarn-cmd">&lt;&lt;SetActive submarine_lost true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start find_submarine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-submarine-done"></a>

## activity_submarine_done

<div class="yarn-node" data-title="activity_submarine_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_20000&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive submarine true&gt;&gt;</span>
<span class="yarn-line">Naprawiłeś to! Dziękuję!</span> <span class="yarn-meta">#line:0ba85d3 </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Teraz przejdź do następnego strażnika.</span> <span class="yarn-meta">#line:09438f6 </span>
<span class="yarn-cmd">&lt;&lt;target tutor_5weeks&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-submarine-lost"></a>

## submarine_lost

<div class="yarn-node" data-title="submarine_lost">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card submarine_nautilus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = $COLLECTED_ITEMS + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory submarine_nautilus add&gt;&gt;</span>
<span class="yarn-line">Świetnie! Teraz oddaj go opiekunowi.</span> <span class="yarn-meta">#line:0757b30 </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_20000&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-submarine"></a>

## submarine

<div class="yarn-node" data-title="submarine">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card submarine_nautilus&gt;&gt;</span>
<span class="yarn-line">ŁÓDŹ PODWODNA porusza się pod wodą.</span> <span class="yarn-meta">#line:06aef1a </span>

</code>
</pre>
</div>

<a id="ys-node-book-20000"></a>

## book_20000

<div class="yarn-node" data-title="book_20000">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card book_20000_leagues_under_the_sea&gt;&gt;</span>
<span class="yarn-line">Ta KSIĄŻKA nosi tytuł „20 000 mil podmorskiej żeglugi”.</span> <span class="yarn-meta">#line:03536a1 </span>
<span class="yarn-line">ŁÓDŹ PODWODNA podróżuje POD WODĄ.</span> <span class="yarn-meta">#line:0dcb855 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-5weeks"></a>

## tutor_5weeks

<div class="yarn-node" data-title="tutor_5weeks">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// --------------------------------------------------------------------------- </span>
<span class="yarn-header-dim">// 5 weeks - BALLOON</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_balloon")&gt;&gt;</span>
<span class="yarn-line">    Znalazłeś balon na ogrzane powietrze!</span> <span class="yarn-meta">#line:0286510</span>
    [MISSING TRANSLATION:     But it's BROKEN in pieces! #shadow:097c3fc]
    [MISSING TRANSLATION:     Can you please fix it? #shadow:02e7812]
    <span class="yarn-cmd">&lt;&lt;inventory hot_air_balloon remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_verne_balloon activity_balloon_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("find_submarine")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card hot_air_balloon&gt;&gt;</span>
<span class="yarn-line">    Pomocy! Czegoś brakuje!</span> <span class="yarn-meta">#line:0e928ab </span>
<span class="yarn-line">    Znajdź balon na ogrzane powietrze</span> <span class="yarn-meta">#line:0a1520b</span>
    [MISSING TRANSLATION:     If you can't find it, USE THE MAP #shadow:05bc5e9]
    <span class="yarn-cmd">&lt;&lt;SetActive balloon_lost true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start find_balloon&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Wróć do mnie później.</span> <span class="yarn-meta">#line:03af9a8 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-balloon-done"></a>

## activity_balloon_done

<div class="yarn-node" data-title="activity_balloon_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_5weeks&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive balloon true&gt;&gt;</span>
[MISSING TRANSLATION: You repaired it! Thank you! #shadow:0ba85d3]
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
[MISSING TRANSLATION: Now go to next guardian. #shadow:09438f6]
<span class="yarn-cmd">&lt;&lt;target tutor_earthmoon&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-balloon-lost"></a>

## balloon_lost

<div class="yarn-node" data-title="balloon_lost">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card hot_air_balloon&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory hot_air_balloon add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = $COLLECTED_ITEMS + 1&gt;&gt;</span>
[MISSING TRANSLATION: Great! Now bring it back to its guardian #shadow:0757b30]
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_5weeks&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-balloon"></a>

## balloon

<div class="yarn-node" data-title="balloon">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card hot_air_balloon&gt;&gt;</span>
<span class="yarn-line">BALONIK unosi się w górę dzięki ciepłemu POWIETRZU.</span> <span class="yarn-meta">#line:0131b99 </span>
<span class="yarn-line">Porusza się z WIATREM.</span> <span class="yarn-meta">#line:09a8c21 </span>

</code>
</pre>
</div>

<a id="ys-node-book-5weeks"></a>

## book_5weeks

<div class="yarn-node" data-title="book_5weeks">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card book_five_weeks_in_a_balloon&gt;&gt;</span>
<span class="yarn-line">Ta KSIĄŻKA nosi tytuł „Pięć tygodni w balonie”.</span> <span class="yarn-meta">#line:0c3d777 </span>
<span class="yarn-line">BALONIK unosi się, ponieważ ciepłe POWIETRZE jest lekkie.</span> <span class="yarn-meta">#line:020d924 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-earthmoon"></a>

## tutor_earthmoon

<div class="yarn-node" data-title="tutor_earthmoon">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// --------------------------------------------------------------------------- </span>
<span class="yarn-header-dim">// EARTH MOON - ROCKET</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_rocket")&gt;&gt;</span>
<span class="yarn-line">    Znalazłeś RAKIETĘ!</span> <span class="yarn-meta">#line:08fed68 </span>
    [MISSING TRANSLATION:     But it's BROKEN in pieces! #shadow:097c3fc]
    [MISSING TRANSLATION:     Can you please fix it? #shadow:02e7812]
    <span class="yarn-cmd">&lt;&lt;inventory space_rocket remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_verne_rocket activity_rocket_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("find_balloon")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card space_rocket&gt;&gt;</span>
<span class="yarn-line">    Ratunku! Nasza najcenniejsza maszyna zaginęła!</span> <span class="yarn-meta">#line:07c4369 </span>
<span class="yarn-line">    Znajdź RAKIETĘ KOSMICZNĄ</span> <span class="yarn-meta">#line:03efbc0 </span>
    [MISSING TRANSLATION:     If you can't find it, USE THE MAP #shadow:05bc5e9]
    <span class="yarn-cmd">&lt;&lt;SetActive rocket_lost true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start find_rocket&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    [MISSING TRANSLATION:     Come back to me later. #shadow:03af9a8]
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-rocket-done"></a>

## activity_rocket_done

<div class="yarn-node" data-title="activity_rocket_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_earthmoon&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive rocket true&gt;&gt;</span>
[MISSING TRANSLATION: You repaired it! Thank you! #shadow:0ba85d3]
<span class="yarn-comment">//&lt;&lt;camera_reset&gt;&gt;</span>
[MISSING TRANSLATION: Now go to next guardian. #shadow:09438f6]
<span class="yarn-cmd">&lt;&lt;target tutor_80days&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-rocket-lost"></a>

## rocket_lost

<div class="yarn-node" data-title="rocket_lost">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card space_rocket&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory space_rocket add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = $COLLECTED_ITEMS + 1&gt;&gt;</span>
[MISSING TRANSLATION: Great! Now bring it back to its guardian #shadow:0757b30]
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_earthmoon&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-rocket"></a>

## rocket

<div class="yarn-node" data-title="rocket">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card space_rocket&gt;&gt;</span>
<span class="yarn-line">To jest RAKIETA kosmiczna.</span> <span class="yarn-meta">#line:0de00ff </span>
<span class="yarn-line">RAKIETA mocno próbuje opuścić ZIEMIĘ.</span> <span class="yarn-meta">#line:06b6d4d </span>

</code>
</pre>
</div>

<a id="ys-node-book-earthmoon"></a>

## book_earthmoon

<div class="yarn-node" data-title="book_earthmoon">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card book_from_earth_to_moon&gt;&gt;</span>
<span class="yarn-line">Juliusz Verne dawno temu wyobrażał sobie RAKIETY i PODRÓŻE kosmiczne.</span> <span class="yarn-meta">#line:0907ccb </span>
<span class="yarn-line">Historia ma ogromną przestrzeń, w której DZIAŁO wystrzeliwuje RAKIETĘ</span> <span class="yarn-meta">#line:05e298d </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-80days"></a>

## tutor_80days

<div class="yarn-node" data-title="tutor_80days">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// --------------------------------------------------------------------------- </span>
<span class="yarn-header-dim">// 80 DAYS - TRAIN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_train")&gt;&gt;</span>
<span class="yarn-line">    Znalazłeś POCIĄG!</span> <span class="yarn-meta">#line:01f76f3 </span>
    [MISSING TRANSLATION:     But it's BROKEN in pieces! #shadow:097c3fc]
    [MISSING TRANSLATION:     Can you please fix it? #shadow:02e7812]
    <span class="yarn-cmd">&lt;&lt;inventory train remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_verne_train activity_train_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("find_balloon")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card train&gt;&gt;</span>
<span class="yarn-line">    Ratunku! Zgubiliśmy POCIĄG!</span> <span class="yarn-meta">#line:08cfbc0 </span>
<span class="yarn-line">    Znajdź STARY POCIĄG PAROWY</span> <span class="yarn-meta">#line:036574d</span>
    [MISSING TRANSLATION:     If you can't find it, USE THE MAP #shadow:05bc5e9]
    <span class="yarn-cmd">&lt;&lt;SetActive train_lost true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start find_train&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    [MISSING TRANSLATION:     Come back to me later. #shadow:03af9a8]
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-train-done"></a>

## activity_train_done

<div class="yarn-node" data-title="activity_train_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_80days&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive train true&gt;&gt;</span>
[MISSING TRANSLATION: You repaired it! Thank you! #shadow:0ba85d3]
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Znalazłeś wszystkie pojazdy!</span> <span class="yarn-meta">#line:084486e </span>
<span class="yarn-line">Wróć do PRZEWODNIKA PO MUZEUM!</span> <span class="yarn-meta">#line:0ee83b2 #task:back_to_museum_guide</span>
<span class="yarn-cmd">&lt;&lt;task_start back_to_museum_guide&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target museum_guide&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-train-lost"></a>

## train_lost

<div class="yarn-node" data-title="train_lost">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory train add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = $COLLECTED_ITEMS + 1&gt;&gt;</span>
[MISSING TRANSLATION: Great! Now bring it back to its guardian #shadow:0757b30]
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_80days&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-train"></a>

## train

<div class="yarn-node" data-title="train">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train&gt;&gt;</span>
<span class="yarn-line">POCIĄG porusza się po SZYNACH.</span> <span class="yarn-meta">#line:0998347 </span>
<span class="yarn-line">Ten stary POCIĄG poruszał się za pomocą PARY.</span> <span class="yarn-meta">#line:0b7b198 </span>
<span class="yarn-line">Dzięki pociągom długie podróże stały się szybsze.</span> <span class="yarn-meta">#line:05130c0 </span>

</code>
</pre>
</div>

<a id="ys-node-book-80days"></a>

## book_80days

<div class="yarn-node" data-title="book_80days">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card book_around_the_world_80_days&gt;&gt;</span>
<span class="yarn-line">Ta KSIĄŻKA nosi tytuł „W 80 dni dookoła świata”.</span> <span class="yarn-meta">#line:03131e3</span>
<span class="yarn-line">Pokazuje wiele miejsc na ZIEMI.</span> <span class="yarn-meta">#line:0bcc84d </span>

</code>
</pre>
</div>

<a id="ys-node-verne-painting"></a>

## verne_painting

<div class="yarn-node" data-title="verne_painting">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// ---------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// OBJECTS</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To jest Juliusz Verne. Był PISARZEM.</span> <span class="yarn-meta">#line:096a3b3 </span>

</code>
</pre>
</div>

<a id="ys-node-verne-house"></a>

## verne_house

<div class="yarn-node" data-title="verne_house">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Juliusz Verne urodził się tutaj, w NANTES.</span> <span class="yarn-meta">#line:003b311 </span>

</code>
</pre>
</div>

<a id="ys-node-map-nantes"></a>

## map_nantes

<div class="yarn-node" data-title="map_nantes">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To jest MAPA NANTES.</span> <span class="yarn-meta">#line:09bcaba </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-visitor"></a>

## spawned_visitor

<div class="yarn-node" data-title="spawned_visitor">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: visitors </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Uwielbiam czytać KSIĄŻKI!</span> <span class="yarn-meta">#line:00f3a57 </span>
<span class="yarn-line">Juliusz Verne wymyślił opowieści science fiction.</span> <span class="yarn-meta">#line:056e79e </span>
<span class="yarn-line">Juliusz Verne napisał WIELE KSIĄŻEK!</span> <span class="yarn-meta">#line:0caca1b </span>
<span class="yarn-line">Ludzie czytają jego KSIĄŻKI w wielu językach!</span> <span class="yarn-meta">#line:0f5f36d </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-visitor"></a>

## spawned_kid_visitor

<div class="yarn-node" data-title="spawned_kid_visitor">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: visitors </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Podoba mi się opowieść o ŚWIECIE.</span> <span class="yarn-meta">#line:0e81901 #card:book_around_the_world_80_days</span>
<span class="yarn-line">ŁÓDŹ PODWODNA Nautilus jest niesamowita.</span> <span class="yarn-meta">#line:08669ce #card:submarine_nautilus</span>
<span class="yarn-line">Chciałbym kiedyś polecieć BALONEM.</span> <span class="yarn-meta">#line:00be1f0 #card:hot_air_balloon</span>
<span class="yarn-line">RAKIETA na KSIĘŻYC wygląda na szybką.</span> <span class="yarn-meta">#line:07ee86b #card:space_rocket</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-guide"></a>

## spawned_guide

<div class="yarn-node" data-title="spawned_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: guides </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Czy wiesz kim był Juliusz Verne?</span> <span class="yarn-meta">#line:00b1d04 </span>
<span class="yarn-line">Tak, on jest PISARZEM!</span> <span class="yarn-meta">#line:07f7b4b </span>
<span class="yarn-line">Nie, kim on jest?</span> <span class="yarn-meta">#line:06fb1cd </span>
<span class="yarn-line">Juliusz Verne był francuskim PISARZEM. Urodził się w pobliżu.</span> <span class="yarn-meta">#line:0c790cf </span>
<span class="yarn-line">Napisał bardzo znane KSIĄŻKI przygodowe.</span> <span class="yarn-meta">#line:0f92a05</span>
<span class="yarn-line">Wiele pomysłów zawartych w jego KSIĄŻKACH stało się rzeczywistością.</span> <span class="yarn-meta">#line:06e1473 </span>

</code>
</pre>
</div>


