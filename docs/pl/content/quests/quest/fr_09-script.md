---
title: Jedzenie i targ (fr_09) - Script
hide:
---

# Jedzenie i targ (fr_09) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-09-the-colors-of-the-marseille-market/28/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1243903291#gid=1243903291)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_09%20Food%20&%20Market/FR_09%20Food%20&%20Market%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_09 | Food &amp; Market (Marseille)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $baker_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $cheesemonger_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $fishmonger_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $greengrocer_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $grocer_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $pirate_completed = false&gt;&gt;</span>
<span class="yarn-line">Witamy na Lazurowym Wybrzeżu, nad Morzem Śródziemnym!</span> <span class="yarn-meta">#line:078e646</span>
<span class="yarn-line">Wybierzmy się na targ i kupmy składniki na tradycyjne danie.</span> <span class="yarn-meta">#line:0462dca </span>
<span class="yarn-cmd">&lt;&lt;target npc_chef&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Gra ukończona! Gratulacje!</span> <span class="yarn-meta">#line:0f5c958 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_proposal&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-proposal"></a>

## quest_proposal

<div class="yarn-node" data-title="quest_proposal">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jakie jest twoje ulubione jedzenie?</span> <span class="yarn-meta">#line:01f78ed </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chef"></a>

## chef

<div class="yarn-node" data-title="chef">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetCollectedItem("COLLECT_THE_INGREDIENTS") &gt;= 9&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump chef_ingredients_done&gt;&gt;</span>
&lt;&lt;elseif GetCollectedItem("COLLECT_THE_INGREDIENTS") &gt; 0 &gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump chef_not_enough&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump chef_welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chef-welcome"></a>

## chef_welcome

<div class="yarn-node" data-title="chef_welcome">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour! Witamy w Marsylii. Jesteśmy na targu!</span> <span class="yarn-meta">#line:02548dd </span>
<span class="yarn-cmd">&lt;&lt;card bouillabaisse&gt;&gt;</span>
<span class="yarn-line">Chcę przygotować dla Ciebie specjalne danie: zupę rybną bouillabaisse!</span> <span class="yarn-meta">#line:0c65de3 </span>
<span class="yarn-line">Ale potrzebuję kilku składników.</span> <span class="yarn-meta">#line:0623733 </span>
<span class="yarn-cmd">&lt;&lt;jump task_ingredients&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients"></a>

## task_ingredients

<div class="yarn-node" data-title="task_ingredients">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Proszę udać się na rynek i kupić składniki przepisu.</span> <span class="yarn-meta">#line:06602c6 </span>
<span class="yarn-cmd">&lt;&lt;detour task_ingredients_desc&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start COLLECT_THE_INGREDIENTS task_ingredients_done&gt;&gt;</span>
<span class="yarn-line">Pamiętaj o manierach!</span> <span class="yarn-meta">#line:02819db</span>
<span class="yarn-line">Powiedz „Bonjour”, aby powitać kogoś,</span> <span class="yarn-meta">#line:04c9d69 </span>
<span class="yarn-line">i "Merci" jako podziękowanie.</span> <span class="yarn-meta">#line:022fd8f </span>
<span class="yarn-cmd">&lt;&lt;area area_full&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-desc"></a>

## task_ingredients_desc

<div class="yarn-node" data-title="task_ingredients_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Zbierz wszystkie składniki potrzebne do przepisu.</span> <span class="yarn-meta">#line:00849bb #task:COLLECT_THE_INGREDIENTS</span>
<span class="yarn-line">Trochę chleba</span> <span class="yarn-meta">#line:0e3c40e </span>
<span class="yarn-line">Ryba</span> <span class="yarn-meta">#line:0c4ae56 </span>
<span class="yarn-line">Pomarańcza</span> <span class="yarn-meta">#line:01e73d1 </span>
<span class="yarn-line">Cytryna</span> <span class="yarn-meta">#line:0ef3a69 </span>
<span class="yarn-line">Pomidor</span> <span class="yarn-meta">#line:0d6f051 </span>
<span class="yarn-line">Trochę mleka</span> <span class="yarn-meta">#line:0dbb71c </span>
<span class="yarn-line">Oliwa z oliwek</span> <span class="yarn-meta">#line:0710b9c </span>
<span class="yarn-line">Pieprz i sól</span> <span class="yarn-meta">#line:08ad40e </span>
<span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chef-not-enough"></a>

## chef_not_enough

<div class="yarn-node" data-title="chef_not_enough">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Potrzebuję więcej składników!</span> <span class="yarn-meta">#line:0c5c6ac</span>
<span class="yarn-line">Upewnij się, że porozmawiasz ze wszystkimi osobami na rynku.</span> <span class="yarn-meta">#line:0060c01</span>
<span class="yarn-line">I weź składniki, które kupiłeś.</span> <span class="yarn-meta">#line:0ea58f3 </span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-done"></a>

## task_ingredients_done

<div class="yarn-node" data-title="task_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target npc_chef&gt;&gt;</span>
<span class="yarn-line">Świetnie! Masz wszystkie składniki.</span> <span class="yarn-meta">#line:0316106 </span>
<span class="yarn-line">Wróć do szefa kuchni.</span> <span class="yarn-meta">#line:05398f2 #task:go_back_chef</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back_chef&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chef-ingredients-done"></a>

## chef_ingredients_done

<div class="yarn-node" data-title="chef_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wspaniale! Masz wszystko.</span> <span class="yarn-meta">#line:0257fc7</span>
<span class="yarn-line">Byłeś bardzo uprzejmy.</span> <span class="yarn-meta">#line:0112e25</span>
<span class="yarn-line">Przygotujmy teraz naszą ucztę!</span> <span class="yarn-meta">#line:0051174</span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_ingredients&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-ingredients"></a>

## activity_match_ingredients

<div class="yarn-node" data-title="activity_match_ingredients">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dopasuj każdą potrawę do jej sprzedawcy.</span> <span class="yarn-meta">#line:0a6e106</span>
<span class="yarn-cmd">&lt;&lt;activity match_ingredients activity_match_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-done"></a>

## activity_match_done

<div class="yarn-node" data-title="activity_match_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_ingredients") &gt; 0&gt;&gt;
<span class="yarn-line">Gratulacje! Udało Ci się dopasować wszystkie elementy.</span> <span class="yarn-meta">#line:01648b2</span>
<span class="yarn-cmd">&lt;&lt;card bouillabaisse&gt;&gt;</span>
<span class="yarn-line">Teraz czas na ugotowanie zupy rybnej!</span> <span class="yarn-meta">#line:0f0f617 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Nie jest idealnie. Spróbuj ponownie!</span> <span class="yarn-meta">#line:007427a </span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_ingredients&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker"></a>

## baker

<div class="yarn-node" data-title="baker">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags:  noRepeatLastLine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_baker") &gt; 0&gt;&gt;
<span class="yarn-line">    Już kupiłeś ode mnie chleb!</span> <span class="yarn-meta">#line:023d379 </span>
<span class="yarn-line">    Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#line:play_again</span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#line:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity hard_money_baker hard_payment_done&gt;&gt;</span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#line:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:09460ce </span>
    <span class="yarn-cmd">&lt;&lt;jump baker_bonjour&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0bf3f32 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Banan!</span> <span class="yarn-meta">#line:0ebf8b2 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-bonjour"></a>

## baker_bonjour

<div class="yarn-node" data-title="baker_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_baker&gt;&gt;</span>
<span class="yarn-line">Dzień dobry! Jestem piekarzem i sprzedaję świeży chleb.</span> <span class="yarn-meta">#line:0c6f41f</span>
<span class="yarn-line">Codziennie wstaję wcześnie rano, żeby piec.</span> <span class="yarn-meta">#line:0f6e48a </span>
<span class="yarn-cmd">&lt;&lt;jump baker_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-question"></a>

## baker_question

<div class="yarn-node" data-title="baker_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: type=Choice</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:00279c8 </span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:00eab87 </span>
    <span class="yarn-cmd">&lt;&lt;jump baker_pay_activity&gt;&gt;</span>
<span class="yarn-line">Ryby i kraby</span> <span class="yarn-meta">#line:08177a5 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomidory, pomarańcze i cytryny</span> <span class="yarn-meta">#line:0a8294a </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Sól, pieprz i olej</span> <span class="yarn-meta">#line:0babba5 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-pay-activity"></a>

## baker_pay_activity

<div class="yarn-node" data-title="baker_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz kwotę wystarczającą do zapłaty.</span> <span class="yarn-meta">#line:0bbf963 </span>
<span class="yarn-cmd">&lt;&lt;activity money_baker baker_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-payment-done"></a>

## baker_payment_done

<div class="yarn-node" data-title="baker_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dodałem Twoje pozycje do tabeli. Dziękuję!</span> <span class="yarn-meta">#line:0567082 </span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:00da30a </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:00cbd60 </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:00cd1cf </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-fisher"></a>

## fisher

<div class="yarn-node" data-title="fisher">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_fishmonger") &gt; 0&gt;&gt;
<span class="yarn-line">    Już kupiłeś ode mnie rybę!</span> <span class="yarn-meta">#line:044d973 </span>
<span class="yarn-line">    Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity hard_money_fishmonger hard_payment_done&gt;&gt;</span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Liceum!</span> <span class="yarn-meta">#line:0d65316 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:089f618 </span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_bonjour&gt;&gt;</span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:06b0535 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-fisher-bonjour"></a>

## fisher_bonjour

<div class="yarn-node" data-title="fisher_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_fishmonger&gt;&gt;</span>
<span class="yarn-line">Dzień dobry! Jestem rybakiem i sprzedaję ryby i kraby.</span> <span class="yarn-meta">#line:04b4a87</span>
<span class="yarn-line">Wszystkie moje rzeczy pochodzą z morza!</span> <span class="yarn-meta">#line:0aa9ce7 </span>
<span class="yarn-cmd">&lt;&lt;jump fisher_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-fisher-question"></a>

## fisher_question

<div class="yarn-node" data-title="fisher_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:04deddc </span>
<span class="yarn-line">Ryby i kraby</span> <span class="yarn-meta">#line:0e562df </span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_pay_activity&gt;&gt;</span>
<span class="yarn-line">Pomidory, pomarańcze i cytryny</span> <span class="yarn-meta">#line:085463e</span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:0604902 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:0c5f144 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-fisher-pay-activity"></a>

## fisher_pay_activity

<div class="yarn-node" data-title="fisher_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz kwotę wystarczającą do zapłaty.</span> <span class="yarn-meta">#line:0995020 </span>
<span class="yarn-cmd">&lt;&lt;activity money_fishmonger fisher_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-fisher-payment-done"></a>

## fisher_payment_done

<div class="yarn-node" data-title="fisher_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dodałem Twoje pozycje do tabeli. Dziękuję!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:02e64ff </span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:02c23e5 </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:080d945 </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Fisherman&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger"></a>

## cheesemonger

<div class="yarn-node" data-title="cheesemonger">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_cheesemonger") &gt; 0&gt;&gt;
<span class="yarn-line">    Już kupiłeś ode mnie mleko!</span> <span class="yarn-meta">#line:090b5cc </span>
<span class="yarn-line">    Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity hard_money_cheesemonger hard_payment_done&gt;&gt;</span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0693ba6 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:022bf02 </span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_bonjour&gt;&gt;</span>
<span class="yarn-line">Pogawędzić!</span> <span class="yarn-meta">#line:0b56a4d </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-question"></a>

## cheesemonger_question

<div class="yarn-node" data-title="cheesemonger_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:03009de </span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:0aa7def </span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_pay_activity&gt;&gt;</span>
<span class="yarn-line">Sól, pieprz i olej</span> <span class="yarn-meta">#line:057f694 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:087919f </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomidory, pomarańcze i cytryny</span> <span class="yarn-meta">#line:067bfab </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-pay-activity"></a>

## cheesemonger_pay_activity

<div class="yarn-node" data-title="cheesemonger_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group:cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz kwotę wystarczającą do zapłaty.</span> <span class="yarn-meta">#line:0f44ea7 </span>
<span class="yarn-cmd">&lt;&lt;activity money_cheesemonger cheesemonger_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-payment-done"></a>

## cheesemonger_payment_done

<div class="yarn-node" data-title="cheesemonger_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group:cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dodałem Twoje pozycje do tabeli. Dziękuję!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:0dd3ac3 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:02a1238 </span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0273de1 </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Cheesemonger&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-bonjour"></a>

## cheesemonger_bonjour

<div class="yarn-node" data-title="cheesemonger_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć! Sprzedaję ser i mleko. Jestem sprzedawcą serów.</span> <span class="yarn-meta">#line:09eb222 </span>
<span class="yarn-line">Używam zarówno mleka krowiego, jak i koziego.</span> <span class="yarn-meta">#line:02f4bc9 </span>
<span class="yarn-cmd">&lt;&lt;card  person_cheesemonger&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump cheesemonger_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer"></a>

## greengrocer

<div class="yarn-node" data-title="greengrocer">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags:  noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_greengrocer") &gt; 0&gt;&gt;
<span class="yarn-line">    Już kupiłeś ode mnie owoce!</span> <span class="yarn-meta">#line:0755f3c </span>
<span class="yarn-line">    Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity hard_money_greengrocer hard_payment_done&gt;&gt;</span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Witam</span> <span class="yarn-meta">#line:00ee67d </span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0a43c28 </span>
    <span class="yarn-cmd">&lt;&lt;detour talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Pociąg!</span> <span class="yarn-meta">#line:02af86a </span>
    <span class="yarn-cmd">&lt;&lt;detour talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:0039ce8 </span>
    <span class="yarn-cmd">&lt;&lt;jump greengrocer_bonjour&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-bonjour"></a>

## greengrocer_bonjour

<div class="yarn-node" data-title="greengrocer_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  person_greengrocer&gt;&gt;</span>
<span class="yarn-line">Cześć! Jestem warzywniakiem. Sprzedaję owoce i warzywa.</span> <span class="yarn-meta">#line:041ade1</span>
<span class="yarn-line">Moje produkty są zawsze świeże!</span> <span class="yarn-meta">#line:0969b87 </span>
<span class="yarn-cmd">&lt;&lt;jump greengrocer_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-pay-activity"></a>

## greengrocer_pay_activity

<div class="yarn-node" data-title="greengrocer_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz kwotę wystarczającą do zapłaty.</span> <span class="yarn-meta">#line:08fc94e </span>
<span class="yarn-cmd">&lt;&lt;activity money_greengrocer greengrocer_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-payment-done"></a>

## greengrocer_payment_done

<div class="yarn-node" data-title="greengrocer_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dodałem Twoje pozycje do tabeli. Dziękuję!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:0ff9361 </span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0741be3 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:023f352 </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Greengrocer&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-question"></a>

## greengrocer_question

<div class="yarn-node" data-title="greengrocer_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:042eb5a </span>
<span class="yarn-line">Ryby i kraby</span> <span class="yarn-meta">#line:0fead1d </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:0879f58 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomidory, pomarańcze i cytryny</span> <span class="yarn-meta">#line:015a2b4 </span>
    <span class="yarn-cmd">&lt;&lt;jump greengrocer_pay_activity&gt;&gt;</span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:0fd3f3a </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer"></a>

## grocer

<div class="yarn-node" data-title="grocer">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags:  noRepeatLastLine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_grocer") &gt; 0&gt;&gt;
<span class="yarn-line">    Już u mnie kupiłeś!</span> <span class="yarn-meta">#line:0348f3c </span>
<span class="yarn-line">    Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity hard_money_grocer hard_payment_done&gt;&gt;</span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0dd992c </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:0623f71 </span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_bonjour&gt;&gt;</span>
<span class="yarn-line">Książka!</span> <span class="yarn-meta">#line:0b4db3b </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-bonjour"></a>

## grocer_bonjour

<div class="yarn-node" data-title="grocer_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_grocer&gt;&gt;</span>
<span class="yarn-line">Cześć! Jestem sprzedawcą spożywczym. Sprzedaję przyprawy i produkty spożywcze.</span> <span class="yarn-meta">#line:0ffbfa4</span>
<span class="yarn-line">Moje produkty możesz wykorzystać w wielu przepisach.</span> <span class="yarn-meta">#line:0c6a554 </span>
<span class="yarn-cmd">&lt;&lt;jump grocer_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-pay-activity"></a>

## grocer_pay_activity

<div class="yarn-node" data-title="grocer_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz kwotę wystarczającą do zapłaty.</span> <span class="yarn-meta">#line:0c80f9e </span>
<span class="yarn-cmd">&lt;&lt;activity money_grocer grocer_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-payment-done"></a>

## grocer_payment_done

<div class="yarn-node" data-title="grocer_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dodałem Twoje pozycje do tabeli. Dziękuję!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:0ce6f8a </span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0e8ec1b </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:062029a </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Grocer&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-question"></a>

## grocer_question

<div class="yarn-node" data-title="grocer_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:0c36100 </span>
<span class="yarn-line">Pomidory, pomarańcze i cytryny</span> <span class="yarn-meta">#line:0d6dabd </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:03eeda4 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:097fca2 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Sól, pieprz i olej</span> <span class="yarn-meta">#line:0068f15 </span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_pay_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pirate"></a>

## pirate

<div class="yarn-node" data-title="pirate">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pirates</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $pirate_completed&gt;&gt;</span>
<span class="yarn-line">A teraz ruszaj! Muszę przygotować się do następnej podróży.</span> <span class="yarn-meta">#line:04a0605 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Ahoj! Witamy na pokładzie.</span> <span class="yarn-meta">#line:04ee922 </span>
<span class="yarn-cmd">&lt;&lt;card pirates&gt;&gt;</span>
<span class="yarn-line">Pochodzimy z Saint-Malo i przepłynęliśmy całe morza, żeby tu dotrzeć.</span> <span class="yarn-meta">#line:056c70d </span>
<span class="yarn-line">Ludzie nazywają nas piratami, ale byliśmy korsarzami.</span> <span class="yarn-meta">#line:0f764a6</span>
<span class="yarn-line">Chcesz zagrać w grę?</span> <span class="yarn-meta">#line:022f719 </span>
<span class="yarn-line">Tak</span> <span class="yarn-meta">#shadow:yes</span>
    <span class="yarn-cmd">&lt;&lt;jump pirate_activity&gt;&gt;</span>
<span class="yarn-line">NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-line">    Ahoj! Niechaj wiatry będą z wami pomyślne!</span> <span class="yarn-meta">#line:0d078d6 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pirate-activity"></a>

## pirate_activity

<div class="yarn-node" data-title="pirate_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: pirates</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Uzupełnij obraz.</span> <span class="yarn-meta">#line:08396f2 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_pirate activity_pirate_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-pirate-done"></a>

## activity_pirate_done

<div class="yarn-node" data-title="activity_pirate_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pirates</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>

&lt;&lt;if GetActivityResult("jigsaw_pirate") &gt; 0&gt;&gt;
<span class="yarn-line">Dobra robota, kolego! Udało ci się rozwiązać zagadkę.</span> <span class="yarn-meta">#line:07e3282 </span>
<span class="yarn-line">Pracowaliśmy dla króla Francji.</span> <span class="yarn-meta">#line:0af3bba </span>
<span class="yarn-line">W przeszłości kradliśmy przedmioty wrogom króla, ale te dni już minęły.</span> <span class="yarn-meta">#line:0b61715 </span>
<span class="yarn-line">Powinieneś zapłacić za to, czego potrzebujesz</span> <span class="yarn-meta">#line:0209784 </span>
<span class="yarn-line">aby sklepy mogły pozostać otwarte!</span> <span class="yarn-meta">#line:02c49d0 </span>
<span class="yarn-line">Przyjmij mój skarb jako nagrodę.</span> <span class="yarn-meta">#line:092a215 </span>
<span class="yarn-cmd">&lt;&lt;set $pirate_completed = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;trigger pirate_chest_lid&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetInteractable pirate_chest false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive pirate_cookie true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Ahoj! Nie udało Ci się rozwiązać zagadki.</span> <span class="yarn-meta">#line:00ec06f </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pirate-chest"></a>

## pirate_chest

<div class="yarn-node" data-title="pirate_chest">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pirates</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $pirate_completed == false&gt;&gt;</span>
<span class="yarn-line">Skrzynia pirata jest zamknięta.</span> <span class="yarn-meta">#line:79f813e5</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-dont-understand"></a>

## talk_dont_understand

<div class="yarn-node" data-title="talk_dont_understand">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przepraszam, chyba nie rozumiem...</span> <span class="yarn-meta">#line:0f9044b </span>
<span class="yarn-line">Co??</span> <span class="yarn-meta">#line:09682b7 </span>
<span class="yarn-line">Co?</span> <span class="yarn-meta">#line:0c1b3e0 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-dont-sell"></a>

## talk_dont_sell

<div class="yarn-node" data-title="talk_dont_sell">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przykro mi, ale tego nie sprzedaję.</span> <span class="yarn-meta">#line:08700b0 </span>

</code>
</pre>
</div>

<a id="ys-node-hard-payment-done"></a>

## hard_payment_done

<div class="yarn-node" data-title="hard_payment_done">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Spróbuj porozmawiać również z innymi sprzedawcami.</span> <span class="yarn-meta">#line:06ae965 </span>

</code>
</pre>
</div>

<a id="ys-node-item-bread"></a>

## item_bread

<div class="yarn-node" data-title="item_bread">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:  item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_bread&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:08e101e </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-fish"></a>

## item_fish

<div class="yarn-node" data-title="item_fish">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:  item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_fish&gt;&gt;</span>
<span class="yarn-line">Ryba</span> <span class="yarn-meta">#line:0feed79</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-crab"></a>

## item_crab

<div class="yarn-node" data-title="item_crab">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:  item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_crab&gt;&gt;</span>
<span class="yarn-line">Krab</span> <span class="yarn-meta">#line:0c81979</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-orange"></a>

## item_orange

<div class="yarn-node" data-title="item_orange">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_orange&gt;&gt;</span>
<span class="yarn-line">Pomarańczowy</span> <span class="yarn-meta">#line:0c0fa04</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-lemon"></a>

## item_lemon

<div class="yarn-node" data-title="item_lemon">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_lemon&gt;&gt;</span>
<span class="yarn-line">Cytrynowy</span> <span class="yarn-meta">#line:0c6b991</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-tomato"></a>

## item_tomato

<div class="yarn-node" data-title="item_tomato">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags:  item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_tomato&gt;&gt;</span>
<span class="yarn-line">Pomidor</span> <span class="yarn-meta">#line:0a6782d</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-milk"></a>

## item_milk

<div class="yarn-node" data-title="item_milk">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags:  item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_milk&gt;&gt;</span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:0acd781</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-pepper-salt"></a>

## item_pepper_salt

<div class="yarn-node" data-title="item_pepper_salt">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:  item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_pepper_salt&gt;&gt;</span>
<span class="yarn-line">Sól i pieprz</span> <span class="yarn-meta">#line:07bbcb0</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-oil"></a>

## item_oil

<div class="yarn-node" data-title="item_oil">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags:  item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_olive_oil&gt;&gt;</span>
<span class="yarn-line">Oliwa z oliwek</span> <span class="yarn-meta">#line:0156410</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć! Przyjeżdżam z Paryża.</span> <span class="yarn-meta">#line:00a142a </span>
<span class="yarn-line">Jedzenie tutaj jest niesamowite!</span> <span class="yarn-meta">#line:0374dff </span>
<span class="yarn-line">Uwielbiam morze!</span> <span class="yarn-meta">#line:0980627 </span>
<span class="yarn-line">Na rynku jest tak wesoło!</span> <span class="yarn-meta">#line:0d92388 </span>
<span class="yarn-line">Bouillabaisse to moje ulubione danie!</span> <span class="yarn-meta">#line:07c080f </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-buyer"></a>

## spawned_buyer

<div class="yarn-node" data-title="spawned_buyer">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: buyers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Muszę kupić świeże składniki.</span> <span class="yarn-meta">#line:0baa74d </span>
<span class="yarn-line">Na rynku można kupić najlepsze produkty.</span> <span class="yarn-meta">#line:042c6f0 </span>
<span class="yarn-line">Świeża ryba jest najlepsza!</span> <span class="yarn-meta">#line:01269c6 </span>
<span class="yarn-line">Nie mogę się doczekać, aż przygotuję pyszny posiłek!</span> <span class="yarn-meta">#line:0fc5cd3 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-currency"></a>

## spawned_currency

<div class="yarn-node" data-title="spawned_currency">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jedno euro to 100 centów.</span> <span class="yarn-meta">#line:0e6c526 #card:currency_euro</span>
<span class="yarn-line">Uprzejme słowa i odpowiednie monety: idealnie!</span> <span class="yarn-meta">#line:07a5934 #card:currency_euro</span>
<span class="yarn-line">Każda waluta euro ma inną wielkość.</span> <span class="yarn-meta">#line:021819a #card:currency_euro</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-recipe"></a>

## spawned_recipe

<div class="yarn-node" data-title="spawned_recipe">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: buyers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bouillabaisse to zupa rybna łącząca wiele gatunków ryb.</span> <span class="yarn-meta">#line:0b6981c #card:bouillabaisse</span>
<span class="yarn-line">Świeży pomidor dodaje smaku.</span> <span class="yarn-meta">#line:0477851 #card:food_tomato</span>
<span class="yarn-line">Oliwa z oliwek dodaje smaku.</span> <span class="yarn-meta">#line:060644d #card:food_olive_oil    </span>
<span class="yarn-line">Chleb świetnie nadaje się do maczania zup.</span> <span class="yarn-meta">#line:04e69ac #card:food_bread</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-jobs"></a>

## spawned_jobs

<div class="yarn-node" data-title="spawned_jobs">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: buyers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Piekarz piecze chleb.</span> <span class="yarn-meta">#line:0606dc5 #card:person_baker</span>
<span class="yarn-line">Sprzedawca ryb sprzedaje ryby i kraby.</span> <span class="yarn-meta">#line:0ca5a9b #card:person_fishmonger</span>
<span class="yarn-line">Sprzedawca sera sprzedaje ser i mleko.</span> <span class="yarn-meta">#line:0b5b1c5 #card:person_cheesemonger</span>
<span class="yarn-line">Sprzedawca warzyw i owoców sprzedaje warzywa.</span> <span class="yarn-meta">#line:03b9a2e #card:person_greengrocer</span>

</code>
</pre>
</div>


