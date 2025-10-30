---
title: Cibo e mercato (fr_09) - Script
hide:
---

# Cibo e mercato (fr_09) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-09-the-colors-of-the-marseille-market/28/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1243903291#gid=1243903291)  
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
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ingredients = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $baker_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $cheesemonger_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $fishmonger_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $greengrocer_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $grocer_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $pirate_completed = false&gt;&gt;</span>
<span class="yarn-line">Benvenuti in Costa Azzurra! È una giornata di sole sulla costa.</span> <span class="yarn-meta">#line:078e646</span>
<span class="yarn-cmd">&lt;&lt;target target_chef&gt;&gt;</span> 

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
<span class="yarn-line">Il gioco è completato! Congratulazioni!</span> <span class="yarn-meta">#line:0f5c958 </span>
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
<span class="yarn-line">Qual è il tuo cibo preferito?</span> <span class="yarn-meta">#line:01f78ed </span>
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
<span class="yarn-cmd">&lt;&lt;if GetCollectedItem("COLLECT_THE_INGREDIENTS") == 9&gt;&gt;</span>
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
<span class="yarn-line">Buongiorno! Benvenuti a Marsiglia, sul Mar Mediterraneo!</span> <span class="yarn-meta">#line:02548dd </span>
<span class="yarn-line">Voglio prepararti un piatto speciale: la bouillabaisse!</span> <span class="yarn-meta">#line:0c65de3 </span>
<span class="yarn-cmd">&lt;&lt;card bouillabaisse zoom&gt;&gt;</span>
<span class="yarn-line">Ma ho bisogno di alcuni ingredienti.</span> <span class="yarn-meta">#line:0623733 </span>
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
<span class="yarn-line">Per favore, vai al mercato e comprali per me.</span> <span class="yarn-meta">#line:06602c6 </span>
<span class="yarn-cmd">&lt;&lt;detour task_ingredients_desc&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start COLLECT_THE_INGREDIENTS task_ingredients_done&gt;&gt;</span>
<span class="yarn-line">Ricordatevi le buone maniere!</span> <span class="yarn-meta">#line:02819db </span>
<span class="yarn-line">Dire "Bonjour" per salutare qualcuno,</span> <span class="yarn-meta">#line:04c9d69 </span>
<span class="yarn-line">e "Merci" per ringraziarli.</span> <span class="yarn-meta">#line:022fd8f </span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-desc"></a>

## task_ingredients_desc

<div class="yarn-node" data-title="task_ingredients_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Raccogli tutti gli ingredienti per la ricetta.</span> <span class="yarn-meta">#line:00849bb #task:COLLECT_THE_INGREDIENTS</span>
[MISSING TRANSLATION: bread #card:food_bread]
[MISSING TRANSLATION: fish #card:food_fish]
[MISSING TRANSLATION: orange #card:food_orange]
[MISSING TRANSLATION: lemon #card:food_lemon]
[MISSING TRANSLATION: tomato #card:food_tomatobread]
[MISSING TRANSLATION: milk #card:food_milk]
[MISSING TRANSLATION: pepper and salt #card:food_pepper_salt]
[MISSING TRANSLATION: oil #card:food_olive_oil]

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
<span class="yarn-line">Ho bisogno di più ingredienti!</span> <span class="yarn-meta">#line:0c5c6ac </span>
<span class="yarn-line">Assicuratevi di parlare con tutti sul mercato.</span> <span class="yarn-meta">#line:0060c01 </span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-done"></a>

## task_ingredients_done

<div class="yarn-node" data-title="task_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai tutti gli ingredienti! Torna dallo chef.</span> <span class="yarn-meta">#line:05398f2 </span>

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
<span class="yarn-line">Magnifico! Hai tutto.</span> <span class="yarn-meta">#line:0257fc7 </span>
<span class="yarn-line">Sei stato molto gentile.</span> <span class="yarn-meta">#line:0112e25 </span>
<span class="yarn-line">Ora prepariamo il nostro banchetto!</span> <span class="yarn-meta">#line:0051174 </span>
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
<span class="yarn-line">Abbina ogni articolo al venditore giusto.</span> <span class="yarn-meta">#line:0a6e106 </span>
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
<span class="yarn-line">Ottimo lavoro! Hai abbinato tutti gli elementi.</span> <span class="yarn-meta">#line:01648b2 </span>
<span class="yarn-line">Adesso prepariamo la bouillabaisse!</span> <span class="yarn-meta">#line:0f0f617 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-bonjour"></a>

## baker_bonjour

<div class="yarn-node" data-title="baker_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buongiorno! Vendo pane fresco. Sono un fornaio.</span> <span class="yarn-meta">#line:0c6f41f </span>
<span class="yarn-line">Ogni giorno mi sveglio presto per cucinare.</span> <span class="yarn-meta">#line:0f6e48a </span>
<span class="yarn-cmd">&lt;&lt;card person_baker zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump baker_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-baker"></a>

## shop_baker

<div class="yarn-node" data-title="shop_baker">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">tags:  noRepeatLastLine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_baker") &gt; 0&gt;&gt;
<span class="yarn-line">    Hai già comprato il pane da me!</span> <span class="yarn-meta">#line:023d379 </span>
<span class="yarn-line">    Vuoi giocare di nuovo?</span> <span class="yarn-meta">#line:042cec8 </span>
    <span class="yarn-cmd">&lt;&lt;activity hard_money_baker hard_payment_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#line:09460ce </span>
    <span class="yarn-cmd">&lt;&lt;jump baker_bonjour&gt;&gt;</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#line:0bf3f32 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Banana!</span> <span class="yarn-meta">#line:0ebf8b2 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-question"></a>

## baker_question

<div class="yarn-node" data-title="baker_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">tags:  type=Choice</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cosa vuoi acquistare?</span> <span class="yarn-meta">#line:00279c8 </span>
<span class="yarn-line">Pane</span> <span class="yarn-meta">#line:00eab87 </span>
    <span class="yarn-cmd">&lt;&lt;jump baker_pay_activity&gt;&gt;</span>
<span class="yarn-line">Pesce e granchio</span> <span class="yarn-meta">#line:08177a5 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomodori, arance e limoni</span> <span class="yarn-meta">#line:0a8294a </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Sale, pepe e olio</span> <span class="yarn-meta">#line:0babba5 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-pay-activity"></a>

## baker_pay_activity

<div class="yarn-node" data-title="baker_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Seleziona denaro sufficiente per pagare.</span> <span class="yarn-meta">#line:0bbf963 </span>
<span class="yarn-cmd">&lt;&lt;activity money_baker baker_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-payment-done"></a>

## baker_payment_done

<div class="yarn-node" data-title="baker_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: no_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco qui.</span> <span class="yarn-meta">#line:0567082 </span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#line:00da30a </span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#line:00cbd60 </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:00cd1cf </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-dontsell"></a>

## baker_dontsell

<div class="yarn-node" data-title="baker_dontsell">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi dispiace, non lo vendo.</span> <span class="yarn-meta">#line:0875143 </span>

</code>
</pre>
</div>

<a id="ys-node-fisher-payment-done"></a>

## fisher_payment_done

<div class="yarn-node" data-title="fisher_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: do_not_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco qui.</span> <span class="yarn-meta">#line:08733ac </span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#line:02e64ff </span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#line:02c23e5 </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:080d945 </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Fisherman&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-fisher-dontsell"></a>

## fisher_dontsell

<div class="yarn-node" data-title="fisher_dontsell">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi dispiace, non lo vendo.</span> <span class="yarn-meta">#line:01ea288 </span>

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
<span class="yarn-line">Cosa vuoi acquistare?</span> <span class="yarn-meta">#line:04deddc </span>
<span class="yarn-line">Pesce e granchio</span> <span class="yarn-meta">#line:0e562df </span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_pay_activity&gt;&gt;</span>
<span class="yarn-line">Pomodori, arance e limoni</span> <span class="yarn-meta">#line:085463e </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pane</span> <span class="yarn-meta">#line:0604902 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Latte</span> <span class="yarn-meta">#line:0c5f144 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-fisher"></a>

## shop_fisher

<div class="yarn-node" data-title="shop_fisher">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_fishmonger") &gt; 0&gt;&gt;
<span class="yarn-line">    Hai già comprato del pesce da me!</span> <span class="yarn-meta">#line:044d973 </span>
<span class="yarn-line">    Vuoi giocare di nuovo?</span> <span class="yarn-meta">#line:0fcca94 </span>
    <span class="yarn-cmd">&lt;&lt;activity hard_money_fishmonger hard_payment_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Liceo!</span> <span class="yarn-meta">#line:0d65316 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#line:089f618 </span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_bonjour&gt;&gt;</span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#line:06b0535 </span>
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
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buongiorno! Vendo pesce e granchi. Sono un pescivendolo.</span> <span class="yarn-meta">#line:04b4a87 </span>
<span class="yarn-line">Tutti i miei articoli provengono dal mare!</span> <span class="yarn-meta">#line:0aa9ce7 </span>
<span class="yarn-cmd">&lt;&lt;card  person_fishmonger zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump fisher_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-cheesemonger"></a>

## shop_cheesemonger

<div class="yarn-node" data-title="shop_cheesemonger">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags:  do_not_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_cheesemonger") &gt; 0&gt;&gt;
<span class="yarn-line">    Hai già comprato il latte da me!</span> <span class="yarn-meta">#line:090b5cc </span>
<span class="yarn-line">    Vuoi giocare di nuovo?</span> <span class="yarn-meta">#line:0432de2 </span>
    <span class="yarn-cmd">&lt;&lt;activity hard_money_cheesemonger hard_payment_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#line:0693ba6 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#line:022bf02 </span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_bonjour&gt;&gt;</span>
<span class="yarn-line">Chiacchierata!</span> <span class="yarn-meta">#line:0b56a4d </span>
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
<span class="yarn-line">Cosa vuoi acquistare?</span> <span class="yarn-meta">#line:03009de </span>
<span class="yarn-line">Latte</span> <span class="yarn-meta">#line:0aa7def </span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_pay_activity&gt;&gt;</span>
<span class="yarn-line">Sale, pepe e olio</span> <span class="yarn-meta">#line:057f694 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pane</span> <span class="yarn-meta">#line:087919f </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomodori, arance e limoni</span> <span class="yarn-meta">#line:067bfab </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-payment-done"></a>

## cheesemonger_payment_done

<div class="yarn-node" data-title="cheesemonger_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group:cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags:  do_not_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco qui.</span> <span class="yarn-meta">#line:07d1671 </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:0dd3ac3 </span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#line:02a1238 </span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#line:0273de1 </span>
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
<span class="yarn-line">Ciao! Vendo formaggio e latte. Sono un casaro.</span> <span class="yarn-meta">#line:09eb222 </span>
<span class="yarn-line">Io uso sia latte vaccino che latte di capra.</span> <span class="yarn-meta">#line:02f4bc9 </span>
<span class="yarn-cmd">&lt;&lt;card  person_cheesemonger zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump cheesemonger_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-dontsell"></a>

## cheesemonger_dontsell

<div class="yarn-node" data-title="cheesemonger_dontsell">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group:cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi dispiace, non lo vendo.</span> <span class="yarn-meta">#line:058fc6d </span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-dontsell"></a>

## greengrocer_dontsell

<div class="yarn-node" data-title="greengrocer_dontsell">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">UOMO: Mi dispiace, non lo vendo.</span> <span class="yarn-meta">#line:03b0024 </span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-payment-activity"></a>

## greengrocer_payment_activity

<div class="yarn-node" data-title="greengrocer_payment_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags:  do_not_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco qui.</span> <span class="yarn-meta">#line:019edd0 </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:0ff9361 </span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#line:0741be3 </span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#line:023f352 </span>
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
<span class="yarn-line">Cosa vuoi acquistare?</span> <span class="yarn-meta">#line:042eb5a </span>
<span class="yarn-line">Pesce e granchio</span> <span class="yarn-meta">#line:0fead1d </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pane</span> <span class="yarn-meta">#line:0879f58 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomodori, arance e limoni</span> <span class="yarn-meta">#line:015a2b4 </span>
    <span class="yarn-cmd">&lt;&lt;jump greengrocer_pay_activity&gt;&gt;</span>
<span class="yarn-line">Latte</span> <span class="yarn-meta">#line:0fd3f3a </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-greengrocer"></a>

## shop_greengrocer

<div class="yarn-node" data-title="shop_greengrocer">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags:  noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_greengrocer") &gt; 0&gt;&gt;
<span class="yarn-line">    Hai già comprato la frutta da me!</span> <span class="yarn-meta">#line:0755f3c </span>
<span class="yarn-line">    Vuoi giocare di nuovo?</span> <span class="yarn-meta">#line:0bbe1ea </span>
    <span class="yarn-cmd">&lt;&lt;activity hard_money_greengrocer hard_payment_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Ciao</span> <span class="yarn-meta">#line:00ee67d </span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#line:0a43c28 </span>
    <span class="yarn-cmd">&lt;&lt;detour talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Treno!</span> <span class="yarn-meta">#line:02af86a </span>
    <span class="yarn-cmd">&lt;&lt;detour talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#line:0039ce8 </span>
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
<span class="yarn-line">Ciao! Vendo frutta e verdura. Sono un fruttivendolo.</span> <span class="yarn-meta">#line:041ade1 </span>
<span class="yarn-line">I miei articoli sono sempre freschi!</span> <span class="yarn-meta">#line:0969b87 </span>
<span class="yarn-cmd">&lt;&lt;card  person_greengrocer zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump greengrocer_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-payment-done"></a>

## grocer_payment_done

<div class="yarn-node" data-title="grocer_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags:  do_not_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ecco qui.</span> <span class="yarn-meta">#line:050a906 </span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#line:0ce6f8a </span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#line:0e8ec1b </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:062029a </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Grocer&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-dontsell"></a>

## grocer_dontsell

<div class="yarn-node" data-title="grocer_dontsell">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">action: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi dispiace, non lo vendo.</span> <span class="yarn-meta">#line:0977493 </span>

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
<span class="yarn-line">Cosa vuoi acquistare?</span> <span class="yarn-meta">#line:0c36100 </span>
<span class="yarn-line">Pomodori, arance e limoni</span> <span class="yarn-meta">#line:0d6dabd </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pane</span> <span class="yarn-meta">#line:03eeda4 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Latte</span> <span class="yarn-meta">#line:097fca2 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Sale, pepe e olio</span> <span class="yarn-meta">#line:0068f15 </span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_pay_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-grocer"></a>

## shop_grocer

<div class="yarn-node" data-title="shop_grocer">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags:  noRepeatLastLine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_grocer") &gt; 0&gt;&gt;
<span class="yarn-line">    Hai già acquistato da me!</span> <span class="yarn-meta">#line:0348f3c </span>
<span class="yarn-line">    Vuoi giocare di nuovo?</span> <span class="yarn-meta">#line:0e4aeb6 </span>
    <span class="yarn-cmd">&lt;&lt;activity hard_money_grocer hard_payment_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#line:0dd992c </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#line:0623f71 </span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_bonjour&gt;&gt;</span>
<span class="yarn-line">Libero!</span> <span class="yarn-meta">#line:0b4db3b </span>
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
<span class="yarn-line">Ciao! Vendo spezie e prodotti per la dispensa. Sono un commerciante di generi alimentari.</span> <span class="yarn-meta">#line:0ffbfa4 </span>
<span class="yarn-line">Puoi usare i miei prodotti per tante ricette.</span> <span class="yarn-meta">#line:0c6a554 </span>
<span class="yarn-cmd">&lt;&lt;card  person_grocer zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump grocer_question&gt;&gt;</span>

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
<span class="yarn-line">Ora vado! Devo prepararmi per il mio prossimo viaggio.</span> <span class="yarn-meta">#line:04a0605 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Ahoy! Benvenuti a bordo.</span> <span class="yarn-meta">#line:04ee922 </span>
<span class="yarn-cmd">&lt;&lt;card pirates&gt;&gt;</span>
<span class="yarn-line">Veniamo da Saint-Malo e abbiamo navigato per arrivare fin qui.</span> <span class="yarn-meta">#line:056c70d </span>
<span class="yarn-line">La gente ci chiama pirati, ma eravamo corsari, corsari.</span> <span class="yarn-meta">#line:0f764a6 </span>
<span class="yarn-cmd">&lt;&lt;jump pirate_activity&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pirate-activity"></a>

## pirate_activity

<div class="yarn-node" data-title="pirate_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: pirates</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Completa l'immagine.</span> <span class="yarn-meta">#line:08396f2 </span>
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
<span class="yarn-line">Lavoravamo per il re di Francia.</span> <span class="yarn-meta">#line:0af3bba </span>
<span class="yarn-line">In passato, prendevamo oggetti dai nemici del re, ma quei giorni sono finiti.</span> <span class="yarn-meta">#line:0b61715 </span>
<span class="yarn-line">Dovresti pagare per quello di cui hai bisogno</span> <span class="yarn-meta">#line:0209784 </span>
<span class="yarn-line">Dovresti pagare per quello di cui hai bisogno affinché i negozi possano restare aperti!</span> <span class="yarn-meta">#line:02c49d0 </span>
<span class="yarn-cmd">&lt;&lt;set $pirate_completed = true&gt;&gt;</span>

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
<span class="yarn-line">Seleziona denaro sufficiente per pagare.</span> <span class="yarn-meta">#line:0995020 </span>
<span class="yarn-cmd">&lt;&lt;activity money_fishmonger fisher_payment_done&gt;&gt;</span>

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
<span class="yarn-line">Seleziona denaro sufficiente per pagare.</span> <span class="yarn-meta">#line:0f44ea7 </span>
<span class="yarn-cmd">&lt;&lt;activity money_cheesemonger cheesemonger_payment_done&gt;&gt;</span>

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
<span class="yarn-line">Seleziona denaro sufficiente per pagare.</span> <span class="yarn-meta">#line:0c80f9e </span>
<span class="yarn-cmd">&lt;&lt;activity money_grocer grocer_payment_done&gt;&gt;</span>

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
<span class="yarn-line">Seleziona denaro sufficiente per pagare.</span> <span class="yarn-meta">#line:08fc94e </span>
<span class="yarn-cmd">&lt;&lt;activity money_greengrocer greengrocer_payment_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-notunderstand"></a>

## baker_notunderstand

<div class="yarn-node" data-title="baker_notunderstand">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi dispiace, non credo di aver capito...</span> <span class="yarn-meta">#line:0db5121 </span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-notunderstand"></a>

## cheesemonger_notunderstand

<div class="yarn-node" data-title="cheesemonger_notunderstand">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi dispiace, non credo di aver capito...</span> <span class="yarn-meta">#line:01a0ec9 </span>

</code>
</pre>
</div>

<a id="ys-node-grocer-notunderstand"></a>

## grocer_notunderstand

<div class="yarn-node" data-title="grocer_notunderstand">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi dispiace, non credo di aver capito...</span> <span class="yarn-meta">#line:0a30381 </span>

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
<span class="yarn-line">Mi dispiace, non credo di aver capito...</span> <span class="yarn-meta">#line:0f9044b </span>
<span class="yarn-line">Che cosa??</span> <span class="yarn-meta">#line:09682b7 </span>
<span class="yarn-line">Eh?</span> <span class="yarn-meta">#line:0c1b3e0 </span>

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
<span class="yarn-line">Mi dispiace, non lo vendo.</span> <span class="yarn-meta">#line:08700b0 </span>

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
<span class="yarn-line">Prova a parlare anche con gli altri venditori.</span> <span class="yarn-meta">#line:06ae965 </span>

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
<span class="yarn-line">Pane</span> <span class="yarn-meta">#line:08e101e </span>

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
<span class="yarn-line">Pescare</span> <span class="yarn-meta">#line:0feed79 </span>

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
<span class="yarn-line">Granchio</span> <span class="yarn-meta">#line:0c81979 </span>

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
<span class="yarn-line">Arancia</span> <span class="yarn-meta">#line:0c0fa04 </span>

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
<span class="yarn-line">Limone</span> <span class="yarn-meta">#line:0c6b991 </span>

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
<span class="yarn-line">Pomodoro</span> <span class="yarn-meta">#line:0a6782d </span>

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
<span class="yarn-line">Latte</span> <span class="yarn-meta">#line:0acd781 </span>

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
<span class="yarn-line">Sale e pepe</span> <span class="yarn-meta">#line:07bbcb0 </span>

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
<span class="yarn-line">Olio d'oliva</span> <span class="yarn-meta">#line:0156410 </span>

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
<span class="yarn-line">Ciao! Sono in visita da Parigi.</span> <span class="yarn-meta">#line:00a142a </span>
<span class="yarn-line">Il cibo qui è fantastico!</span> <span class="yarn-meta">#line:0374dff </span>
<span class="yarn-line">Adoro il mare!</span> <span class="yarn-meta">#line:0980627 </span>
<span class="yarn-line">Il mercato è così vivace!</span> <span class="yarn-meta">#line:0d92388 </span>
<span class="yarn-line">La bouillabaisse è il mio piatto preferito!</span> <span class="yarn-meta">#line:07c080f </span>

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
<span class="yarn-line">Ho bisogno di comprare degli ingredienti freschi.</span> <span class="yarn-meta">#line:0baa74d </span>
<span class="yarn-line">Il mercato ha i prodotti migliori.</span> <span class="yarn-meta">#line:042c6f0 </span>
<span class="yarn-line">Il pesce fresco è il migliore!</span> <span class="yarn-meta">#line:01269c6 </span>
<span class="yarn-line">Non vedo l'ora di cucinare un pasto delizioso!</span> <span class="yarn-meta">#line:0fc5cd3 </span>

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
<span class="yarn-line">Un euro equivale a 100 centesimi.</span> <span class="yarn-meta">#line:0e6c526 #card:currency_euro</span>
<span class="yarn-line">Parole educate e monete corrette: perfetto!</span> <span class="yarn-meta">#line:07a5934 #card:currency_euro</span>
<span class="yarn-line">Gli euro hanno dimensioni diverse per ogni valore.</span> <span class="yarn-meta">#line:021819a #card:currency_euro</span>

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
<span class="yarn-line">La bouillabaisse mescola molti pesci.</span> <span class="yarn-meta">#line:0b6981c #card:bouillabaisse</span>
<span class="yarn-line">Il pomodoro fresco aggiunge sapore.</span> <span class="yarn-meta">#line:0477851 #card:food_tomato</span>
<span class="yarn-line">L'olio d'oliva aggiunge sapore.</span> <span class="yarn-meta">#line:060644d #card:food_olive_oil    </span>
<span class="yarn-line">Il pane è ottimo per intingere la zuppa.</span> <span class="yarn-meta">#line:04e69ac #card:food_bread</span>

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
<span class="yarn-line">Un fornaio cuoce il pane.</span> <span class="yarn-meta">#line:0606dc5 #card:person_baker</span>
<span class="yarn-line">Un pescivendolo vende pesce e granchi.</span> <span class="yarn-meta">#line:0ca5a9b #card:person_fishmonger</span>
<span class="yarn-line">Un casaro vende formaggio e latte.</span> <span class="yarn-meta">#line:0b5b1c5 #card:person_cheesemonger</span>
<span class="yarn-line">Un fruttivendolo vende frutta e verdura.</span> <span class="yarn-meta">#line:03b9a2e #card:person_greengrocer</span>

</code>
</pre>
</div>


