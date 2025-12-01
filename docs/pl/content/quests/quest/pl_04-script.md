---
title: Zoo (pl_04) - Script
hide:
---

# Zoo (pl_04) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-04-the-zoo/35/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=819047762#gid=819047762)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_04%20Zoo/PL_04%20Zoo%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_04 | Zoo (Wroclaw)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: ADULT_M, </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $elephant_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $giraffe_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lion_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $monkey_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $penguin_completed = false&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;declare $talked_animals = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $found_chest_cookies = false&gt;&gt;</span>
<span class="yarn-line">Witamy w ZOO WE WROCŁAWIU.</span> <span class="yarn-meta">#line:0fe55d1 </span>
<span class="yarn-line">Największe zoo w Polsce!</span> <span class="yarn-meta">#line:005dd46</span>
<span class="yarn-cmd">&lt;&lt;target director&gt;&gt;</span>

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
<span class="yarn-line">To zadanie zostało ukończone.</span> <span class="yarn-meta">#line:0bcc257 </span>
<span class="yarn-line">Dowiedziałeś się o ZWIERZĘTACH ZOOLOGICZNYCH.</span> <span class="yarn-meta">#line:054cc37 </span>
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
<span class="yarn-cmd">&lt;&lt;card wroclaw_centennial_hall&gt;&gt;</span>
<span class="yarn-line">Narysuj swoje ulubione ZWIERZĘ lub SALĘ STULECIA.</span> <span class="yarn-meta">#line:0809ac5</span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director"></a>

## director

<div class="yarn-node" data-title="director">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("TASK_ANIMALS")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump director_task_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump director_task&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director-task"></a>

## director_task

<div class="yarn-node" data-title="director_task">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
<span class="yarn-line">Cześć, jestem dyrektorem zoo.</span> <span class="yarn-meta">#line:0815405 </span>
<span class="yarn-line">Dzisiaj mamy problem.</span> <span class="yarn-meta">#line:0c71176 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus Flagpole&gt;&gt;</span>
<span class="yarn-line">Nasza FLAGA zaginęła!</span> <span class="yarn-meta">#line:09c6bf7 </span>
<span class="yarn-cmd">&lt;&lt;card iglica&gt;&gt;</span>
<span class="yarn-line">Miało to miejsce na IGLICA w CENTENNIAL HALL.</span> <span class="yarn-meta">#line:02f35e4 </span>
<span class="yarn-line">To słynna metalowa rzeźba i symbol naszego miasta.</span> <span class="yarn-meta">#line:0335bf7 </span>
<span class="yarn-line">Ma 90 metrów wysokości!</span> <span class="yarn-meta">#line:001cba3 </span>
<span class="yarn-line">Kto mógł to zabrać?</span> <span class="yarn-meta">#line:0fc7e35 </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Znajdź FLAGĘ. Porozmawiaj ze ZWIERZĘTAMI.</span> <span class="yarn-meta">#line:0da284c #task:TASK_ANIMALS</span>
<span class="yarn-line">Może któryś z nich je ma.</span> <span class="yarn-meta">#line:012b933</span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_ANIMALS task_animals_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-animals-done"></a>

## task_animals_done

<div class="yarn-node" data-title="task_animals_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Rozmawiałeś ze WSZYSTKIMI ZWIERZĘTAMI.</span> <span class="yarn-meta">#shadow:032811f</span>
<span class="yarn-line">Teraz wróć do DYREKTORA ZOO.</span> <span class="yarn-meta">#line:0a93d9b #task:back_to_director</span>
<span class="yarn-cmd">&lt;&lt;target director&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start back_to_director&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $talked_animals = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director-task-done"></a>

## director_task_done

<div class="yarn-node" data-title="director_task_done">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Rozmawiałeś ze WSZYSTKIMI ZWIERZĘTAMI.</span> <span class="yarn-meta">#line:032811f </span>
<span class="yarn-line">I nie zabrali FLAGĘ.</span> <span class="yarn-meta">#line:0364f30 </span>
<span class="yarn-line">Wróćmy do Hali Stulecia.</span> <span class="yarn-meta">#line:0bc6238 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_zoo_settings jigsaw_zoo_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-jigsaw-zoo-done"></a>

## jigsaw_zoo_done

<div class="yarn-node" data-title="jigsaw_zoo_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Gdyby wszystkie ZWIERZĘTA były niewinne...</span> <span class="yarn-meta">#line:0bc2b46 </span>
<span class="yarn-line">Kto wziął FLAGĘ?</span> <span class="yarn-meta">#line:0fc0ab7 </span>
<span class="yarn-cmd">&lt;&lt;SetActive anturaFlag&gt;&gt;</span>
<span class="yarn-line">Poczekaj, spójrz!</span> <span class="yarn-meta">#line:0b3d05f </span>
<span class="yarn-cmd">&lt;&lt;camera_focus Antura&gt;&gt;</span>
<span class="yarn-line">Antura ma FLAGĘ!</span> <span class="yarn-meta">#line:0e24973 </span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_flag&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive iglica_flag true&gt;&gt;</span>
<span class="yarn-line">Zagadka rozwiązana.</span> <span class="yarn-meta">#line:0bf024f</span>
<span class="yarn-cmd">&lt;&lt;camera_focus Flagpole&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_centennial_hall&gt;&gt;</span>
<span class="yarn-line">Teraz FLAGA powraca.</span> <span class="yarn-meta">#line:0d91701 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-monkey"></a>

## monkey

<div class="yarn-node" data-title="monkey">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ########################## MONKEY</span>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_monkey_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#line:play_again</span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#line:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity match_monkey_settings&gt;&gt;</span>
<span class="yarn-line">    Tak, ale trudniej</span> <span class="yarn-meta">#line:yes_harder</span>
        <span class="yarn-cmd">&lt;&lt;activity match_monkey_settings_hard&gt;&gt;</span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#line:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jestem SZYMPANSEM. Wspinam się na drzewa.</span> <span class="yarn-meta">#line:0867233 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_chimpanzee&gt;&gt;</span>
<span class="yarn-line">    Naprawdę uwielbiam jeść owoce!</span> <span class="yarn-meta">#line:0eaefd6 </span>
<span class="yarn-line">    Czy potrafisz zgadnąć, na które drzewo muszę się wspiąć, aby...</span> <span class="yarn-meta">#line:0617de7 </span>
<span class="yarn-line">    Jabłko</span> <span class="yarn-meta">#line:0c6c41a </span>
<span class="yarn-line">    Pomarańcza</span> <span class="yarn-meta">#line:08c7ea3 </span>
<span class="yarn-line">    A banan?</span> <span class="yarn-meta">#line:0ce7b11 </span>
<span class="yarn-line">    Dopasuj owoce do odpowiednich drzew!</span> <span class="yarn-meta">#line:079451b</span>
    <span class="yarn-cmd">&lt;&lt;activity match_monkey_settings monkey_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-monkey-activity-done"></a>

## monkey_activity_done

<div class="yarn-node" data-title="monkey_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: monkey_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_chimpanzee collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $monkey_completed = true&gt;&gt;</span>
<span class="yarn-line">Byłoby fajnie wspiąć się na ten słup.</span> <span class="yarn-meta">#line:0a43c85</span>
<span class="yarn-line">Ale nie wziąłem FLAGĘ!</span> <span class="yarn-meta">#line:0c53945 </span>

</code>
</pre>
</div>

<a id="ys-node-monkey-tutor"></a>

## monkey_tutor

<div class="yarn-node" data-title="monkey_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_chimpanzee&gt;&gt;</span>
<span class="yarn-line">Szympansy są świetnymi wspinaczami.</span> <span class="yarn-meta">#line:01d80fc </span>
<span class="yarn-cmd">&lt;&lt;card food_apple&gt;&gt;</span>
<span class="yarn-line">Jedzą dużo owoców.</span> <span class="yarn-meta">#line:0883b4b </span>
<span class="yarn-cmd">&lt;&lt;card plant_apple&gt;&gt;</span>
<span class="yarn-line">Każdy owoc rośnie na innym drzewie.</span> <span class="yarn-meta">#line:0cda3c2 </span>

</code>
</pre>
</div>

<a id="ys-node-monkey-kid"></a>

## monkey_kid

<div class="yarn-node" data-title="monkey_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">SZYMPANSY mają długie ramiona, dzięki którym mogą huśtać się na drzewach.</span> <span class="yarn-meta">#line:0bf2346 </span>
<span class="yarn-line">SZYMPANSY są inteligentne!</span> <span class="yarn-meta">#line:0eecdf9 </span>

</code>
</pre>
</div>

<a id="ys-node-lion"></a>

## lion

<div class="yarn-node" data-title="lion">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ##########################  LION</span>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("order_lion_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity order_lion_settings&gt;&gt;</span>
<span class="yarn-line">    Tak, ale trudniej</span> <span class="yarn-meta">#shadow:yes_harder</span>
        <span class="yarn-cmd">&lt;&lt;activity order_lion_settings_hard&gt;&gt;</span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jestem LWEM z AFRYKI.</span> <span class="yarn-meta">#line:07f2e15 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_lion_cub&gt;&gt;</span>
<span class="yarn-line">    Kiedy byłem młody, mieszkałem z rodzicami.</span> <span class="yarn-meta">#line:0124e1c</span>
    <span class="yarn-cmd">&lt;&lt;card animal_lion_young_male&gt;&gt;</span>
<span class="yarn-line">    Potem dorosłem...</span> <span class="yarn-meta">#line:019539f </span>
    <span class="yarn-cmd">&lt;&lt;card animal_lion&gt;&gt;</span>
<span class="yarn-line">    Teraz jestem dorosłym LWEM z wspaniałą grzywą.</span> <span class="yarn-meta">#line:0774b92 </span>
<span class="yarn-line">    Czy możesz ułożyć moje zdjęcia w kolejności?</span> <span class="yarn-meta">#line:095a71f </span>
    <span class="yarn-cmd">&lt;&lt;activity order_lion_settings lion_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-lion-activity-done"></a>

## lion_activity_done

<div class="yarn-node" data-title="lion_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $lion_completed = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card animal_lion collect&gt;&gt;</span>
<span class="yarn-line">Uwielbiam patrzeć na flagę na Iglicy.</span> <span class="yarn-meta">#line:01b3593 </span>
<span class="yarn-line">Ale jej nie wziąłem. Proszę, znajdź tę FLAGĘ!</span> <span class="yarn-meta">#line:05da6d7 </span>

</code>
</pre>
</div>

<a id="ys-node-lion-tutor"></a>

## lion_tutor

<div class="yarn-node" data-title="lion_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_lion&gt;&gt;</span>
<span class="yarn-line">Lwy żyją w grupach rodzinnych.</span> <span class="yarn-meta">#line:091881b </span>
<span class="yarn-line">Dorosłe samce mają grzywę.</span> <span class="yarn-meta">#line:02b4a06 </span>
<span class="yarn-cmd">&lt;&lt;card animal_lion_cub&gt;&gt;</span>
<span class="yarn-line">Ale młode nie.</span> <span class="yarn-meta">#line:0d378cc </span>

</code>
</pre>
</div>

<a id="ys-node-lion-kid"></a>

## lion_kid

<div class="yarn-node" data-title="lion_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cóż za ryk!</span> <span class="yarn-meta">#line:079f4e0 </span>
<span class="yarn-line">Lwy są królami dżungli!</span> <span class="yarn-meta">#line:020c29f </span>

</code>
</pre>
</div>

<a id="ys-node-giraffe"></a>

## giraffe

<div class="yarn-node" data-title="giraffe">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ##########################  GIRAFFE</span>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("canvas_giraffe_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity canvas_giraffe_settings&gt;&gt;</span>
<span class="yarn-line">    Tak, ale trudniej</span> <span class="yarn-meta">#shadow:yes_harder</span>
        <span class="yarn-cmd">&lt;&lt;activity canvas_giraffe_settings_hard&gt;&gt;</span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jestem najwyższym ZWIERZĘCIEM.</span> <span class="yarn-meta">#line:0d5c607 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_giraffe&gt;&gt;</span>
<span class="yarn-line">    Moja długa SZYJA pomaga mi dosięgnąć liści.</span> <span class="yarn-meta">#line:0a4d24e </span>
<span class="yarn-line">    Uwielbiam jeść liście akacji!</span> <span class="yarn-meta">#line:04b42f2 </span>
<span class="yarn-line">    Czy możesz wyczyścić dla mnie to zdjęcie?</span> <span class="yarn-meta">#line:02af551 </span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_giraffe_settings giraffe_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-activity-done"></a>

## giraffe_activity_done

<div class="yarn-node" data-title="giraffe_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_giraffe collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $giraffe_completed = true&gt;&gt;</span>
<span class="yarn-line">Dziękuję. Nie wziąłem FLAG.</span> <span class="yarn-meta">#line:0877d6f</span>
<span class="yarn-line">Jestem wysoki, ale nie mam 90 metrów jak IGLICA!</span> <span class="yarn-meta">#line:02d00e2</span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-tutor"></a>

## giraffe_tutor

<div class="yarn-node" data-title="giraffe_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_giraffe&gt;&gt;</span>
<span class="yarn-line">Żyrafy są najwyższymi zwierzętami.</span> <span class="yarn-meta">#line:03b36c0 </span>
<span class="yarn-line">Mają długie szyje umożliwiające im zjadanie liści z wysokich drzew.</span> <span class="yarn-meta">#line:0d9d52f </span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-kid"></a>

## giraffe_kid

<div class="yarn-node" data-title="giraffe_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dzięki takiej SZYI mogłem stąd widzieć swój dom!</span> <span class="yarn-meta">#line:0bee484 </span>

</code>
</pre>
</div>

<a id="ys-node-elephant"></a>

## elephant

<div class="yarn-node" data-title="elephant">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ##########################  ELEPHANT</span>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: elephant_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $elephant_completed&gt;&gt;</span>
<span class="yarn-line">    Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity memory_elephant_settings&gt;&gt;</span>
<span class="yarn-line">    Tak, ale trudniej</span> <span class="yarn-meta">#shadow:yes_harder</span>
        <span class="yarn-cmd">&lt;&lt;activity memory_elephant_settings_hard&gt;&gt;</span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jestem największym ZWIERZĘCIEM lądowym.</span> <span class="yarn-meta">#line:027b51f </span>
    <span class="yarn-cmd">&lt;&lt;card animal_elephant&gt;&gt;</span>
<span class="yarn-line">    Mam świetną PAMIĘĆ.</span> <span class="yarn-meta">#line:03a150c </span>
<span class="yarn-line">    Pamiętam wszystko!</span> <span class="yarn-meta">#line:01fe852 </span>
<span class="yarn-line">    Czy masz dobrą PAMIĘĆ?</span> <span class="yarn-meta">#line:0f98478 </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_elephant_settings elephant_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-elephant-activity-done"></a>

## elephant_activity_done

<div class="yarn-node" data-title="elephant_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: elephant_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_elephant collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $elephant_completed = true&gt;&gt;</span>
<span class="yarn-line">FLAGA? Nie mam jej.</span> <span class="yarn-meta">#line:0b79d01</span>
<span class="yarn-line">Gdybym to wzięła, PAMIĘTAŁABYM!</span> <span class="yarn-meta">#line:0f124bf</span>

</code>
</pre>
</div>

<a id="ys-node-elephant-tutor"></a>

## elephant_tutor

<div class="yarn-node" data-title="elephant_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_elephant&gt;&gt;</span>
<span class="yarn-line">Słonie mają grubą skórę i duże uszy.</span> <span class="yarn-meta">#line:06c3bba </span>
<span class="yarn-line">Są to największe zwierzęta lądowe.</span> <span class="yarn-meta">#line:0bc4ca4 </span>

</code>
</pre>
</div>

<a id="ys-node-elephant-kid"></a>

## elephant_kid

<div class="yarn-node" data-title="elephant_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jego USZY są większe ode mnie!</span> <span class="yarn-meta">#line:0fc78ad </span>
<span class="yarn-line">Czy te uszy mogą mnie wachlować latem?</span> <span class="yarn-meta">#line:004abc7 </span>

</code>
</pre>
</div>

<a id="ys-node-penguin"></a>

## penguin

<div class="yarn-node" data-title="penguin">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ##########################  PENGUIN</span>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("jigsaw_penguin_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity jigsaw_penguin_settings&gt;&gt;</span>
<span class="yarn-line">    Tak, ale trudniej</span> <span class="yarn-meta">#shadow:yes_harder</span>
        <span class="yarn-cmd">&lt;&lt;activity jigsaw_penguin_settings_hard&gt;&gt;</span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jestem PINGWINEM, wyjątkowym PTAKEM.</span> <span class="yarn-meta">#line:08c70e8 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_penguin&gt;&gt;</span>
<span class="yarn-line">    Nie umiem LATAĆ, ale dobrze PŁYWAM!</span> <span class="yarn-meta">#line:0540c5a </span>
<span class="yarn-line">    Uwielbiam LÓD i ZIMNO.</span> <span class="yarn-meta">#line:0ba1a43 </span>
<span class="yarn-line">    Spróbujmy rozwiązać tę zagadkę razem!</span> <span class="yarn-meta">#line:020100a </span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_penguin_settings penguin_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-penguin-activity-done"></a>

## penguin_activity_done

<div class="yarn-node" data-title="penguin_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: penguin_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_penguin collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $penguin_completed = true&gt;&gt;</span>
<span class="yarn-line">Nie, nie wziąłem FLAG-a.</span> <span class="yarn-meta">#line:078190f </span>
<span class="yarn-line">Pamiętaj, nie potrafię latać!</span> <span class="yarn-meta">#line:08568f5 </span>

</code>
</pre>
</div>

<a id="ys-node-penguin-tutor"></a>

## penguin_tutor

<div class="yarn-node" data-title="penguin_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_penguin&gt;&gt;</span>
<span class="yarn-line">Pingwiny to ptaki, które potrafią pływać.</span> <span class="yarn-meta">#line:0920adc </span>
<span class="yarn-cmd">&lt;&lt;card ice_arctic&gt;&gt;</span>
<span class="yarn-line">Żyją w pobliżu oceanów i lodu.</span> <span class="yarn-meta">#line:0baa8d0 </span>

</code>
</pre>
</div>

<a id="ys-node-penguin-kid"></a>

## penguin_kid

<div class="yarn-node" data-title="penguin_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wolisz latać czy pływać?</span> <span class="yarn-meta">#line:0ae73f3 </span>
<span class="yarn-line">Nie mógłbym żyć na lodzie!</span> <span class="yarn-meta">#line:05ac327 </span>

</code>
</pre>
</div>

<a id="ys-node-spawn-tourist"></a>

## spawn_tourist

<div class="yarn-node" data-title="spawn_tourist">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SPAWN</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  wroclaw_zoo&gt;&gt;</span>
<span class="yarn-line">Jest tu ponad 10 000 ZWIERZĄT.</span> <span class="yarn-meta">#line:0354bea </span>
<span class="yarn-line">To zoo jest bardzo duże.</span> <span class="yarn-meta">#line:0d80278 </span>
<span class="yarn-line">Chyba gdzieś widziałem skrzynię ze skarbami.</span> <span class="yarn-meta">#line:0694a4c </span>

</code>
</pre>
</div>

<a id="ys-node-animal-peacock"></a>

## animal_peacock

<div class="yarn-node" data-title="animal_peacock">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card peacock&gt;&gt;</span>
<span class="yarn-line">    Nie potrzebuję FLAG-u.</span> <span class="yarn-meta">#line:0085a8a </span>
<span class="yarn-line">    Mój ogon jest jak FLAGA!</span> <span class="yarn-meta">#line:04fff6b </span>

</code>
</pre>
</div>

<a id="ys-node-animal-parrot"></a>

## animal_parrot

<div class="yarn-node" data-title="animal_parrot">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card parrot&gt;&gt;</span>
<span class="yarn-line">Krzyk! Widziałem, jak przejeżdżają czerwony i żółty!</span> <span class="yarn-meta">#line:0e84545 </span>

</code>
</pre>
</div>

<a id="ys-node-animal-fox"></a>

## animal_fox

<div class="yarn-node" data-title="animal_fox">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">actor: SPECIAL</span>
<span class="yarn-header-dim">image: zoo_gate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_fox&gt;&gt;</span>
<span class="yarn-line">Hehe! Tak naprawdę nie jestem z zoo.</span> <span class="yarn-meta">#line:03c4553 </span>
<span class="yarn-line">Nigdzie nie widziałem żadnej flagi.</span> <span class="yarn-meta">#line:074fe29 </span>

</code>
</pre>
</div>

<a id="ys-node-item-chest"></a>

## item_chest

<div class="yarn-node" data-title="item_chest">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">group:</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $found_chest_cookies&gt;&gt;</span>
<span class="yarn-line">    Teraz jest pusto.</span> <span class="yarn-meta">#line:0810771 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $found_chest_cookies = true&gt;&gt;</span>
<span class="yarn-line">    Znalazłeś ciasteczka!</span> <span class="yarn-meta">#line:03321d5 </span>
    <span class="yarn-cmd">&lt;&lt;cookies_add 5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>


