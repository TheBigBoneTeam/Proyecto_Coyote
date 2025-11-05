# PROYECTO COYOTE
**GAME DESIGN DOCUMENT**

Eduardo Almarza Blasco • Antonio Bernal de Celis • David del Castillo Enríquez • Diego Fernández Manso • Candela Jiménez González • Andrea Luengo Zazo

Version 1.0


# 0. Introducción
**Este documento presenta el diseño y desarrollo del videojuego “Proyecto Coyote”, donde se expondrá el proceso creativo a lo largo del tiempo hasta su lanzamiento final.**

“Proyecto Coyote” es un juego de acción frenética en 3D con cámara en tercera persona, centrado en la gestión de múltiples enemigos y el combate cuerpo a cuerpo. 

# 1. Concepto del juego 
"Proyecto Coyote” es un videojuego donde el jugador tomará el papel de un vaquero que quiere cumplir la última voluntad de su marido enterrando sus cenizas en un oasis cercano al pueblo donde nació, Pricklytown. Su viaje se complicará al llegar al poblado, que ha sido amenazado por una misteriosa infección desconocida que provoca mutaciones con forma de cactus en el cuerpo de quienes consumen el “sagrado” higo chumbo. 

# 2. Historia
## 2.1. Ambientación 
"Proyecto Coyote” se desarrolla en un viejo oeste fantástico y oscuro. En este mundo existen magias como la necromancia, criaturas sobrenaturales, animales antropomórficos, entre otros. 

La zona donde se desarrolla el videojuego es un área poblada en el desierto que se ha visto afectado por una misteriosa enfermedad que provoca mutaciones en el cuerpo y comportamientos agresivos.  

### Zonas de "Proyecto Coyote"

  ####  Pricklytown
  El protagonista llega inicialmente a Pricklytown, el único poblado de la zona. Se trata de un pequeño burgo muy aislado, habitado por diversas criaturas de personalidades extravagantes. Hay unas pocas viviendas, comercios variados, un peculiar cantina "El Revólver Revoltoso" y un cochambroso hostal para viajeros desafortunados que acaban en Pricklytown.

  ####  El Cañón
  Tras su visita por Pricklytown el protagonista se ve obligado a viajar hacia el oasis del sur, accesible únicamente a través de un cañón. Se trata de una zona desértica, seca y vacía. Hay algunas edificaciones de madera y carros abandonados, ya que esta ruta era empleada por los habitantes de Pricklytown para transportar agua antes de la construcción del pozo.

  ####  El Oasis
  El Oasis es una pequeña zona misteriosa y excesivamente frondosa. Los habitantes de Pricklytown afirman que, a pesar de contener agua, en el pasado prácticamente ninguna planta crecía en la zona, y que solo se conocía como oasis por tradición. Ahora es una exhuberante y densa jungla de todo tipo de cactus y plantas desérticas. En el oasis además hay una edificación puntiaguda que se asemeja a una iglesia.

  *Estos lugares, en cuestiones de gameplay, se traducen directamente como niveles o zonas de progreso que el jugador deberá atravesar*


## 2.2. Trasfondo
>*Existen manuscritos de eras ancestrales que teorizan sobre la existencia de unas cápsulas o huevos que surgieron con la creación del universo. En estas escrituras se les atribuye a estos huevos la capacidad de engendrar vida y alterar el entorno de su alrededor, consiguiendo habitar planetas yermos por completo.* 

### La sequía

En el periodo histórico actual, los habitantes de Pricklytown, un pequeño poblado en el desierto, excavaron un canal subterráneo para facilitar la llegada de agua desde un pequeño oasis cercano. Esta excavación interfirió con un artefacto extraño, de aspecto alienígena y orgánico que los mineros fueron incapaces de retirar y acabaron ignorando. 

Una vez inaugurado el canal, Pricklytown disfrutó de un corto periodo de abundancia gracias al nuevo acceso al manantial. Esta exhuberancia fue breve, pronto el agua escaseó, y tras una expedición, los vecinos del poblado descubrieron un bloqueo de vegetación en el oasis. 

Antes triste y prácticamente árido, el oasis se había transformado ahora en una jungla frondosa de vegetación desértica, con cactus y plantas que bloqueaban el aceso al agua. Este descubrimiento tenía un lado positivo para los vecinos, que pasaban por un periodo de hambruna, ya que este rápido crecimiento también había causado la fructificación de unos jugosos higos en los cactus a los que los habitantes de Pricklytown no pudieron resistirse.


Con el paso del tiempo las criaturas de la zona comenzaron a presentar malestar, aparentemente, debido al consumo del higo chumbo. Aquel que se alimentaba de los higos presentaba síntomas como deshidratación severa, cansancio y fiebre. Con el paso del tiempo, bultos y quistes verdosos brotaban en la piel y pronto se convertían en protuberancias vegetales y carnosas. 

### La expedición
El pueblo organizó una expedición formada por matasanos, voluntarios y miembros de la iglesia para investigar el cambiado oasis. Semanas después de su partida, menos de la mitad regresaron. Los expedicionarios parecían haber enloquecido y afirmaban que el consumo de el higo permitía a cualquier criatura alcanzar la inmortalidad.

Finalmente la misión fue considerada fallida y se atribuyeron los delirios de los supervivientes a la deshidratación.

### La llamada
Con el grave periodo de escasez al que se enfrentaba Pricklytown y la incomprensibilidad general de los efectos de los higos, resultó imposible evitar que los vecinos se alimentaran de ellos.
Quienes los consumían empeoraban con rapidez, y junto con los desagradables síntomas aparecían actitudes agresivas y destructivas. Los "infectados", como fueron etiquetados, eran trasladados recurrentemente al oasis por los pocos que resistían.

El acceso a la jungla de cactus era cada vez más complicado. Según algunos transportistas, desde las afueras de la zona vegetal se escuchaban cánticos inentendibles, pero que parecían atraer con un extraño magnetismo a quienes los escuchaban. Quienes eran capaces de ignorar la llamada la comparaban con la mitología sobre el canto de las sirenas, pero negaban su sobrenaturalidad y atrubuían la atracción al agua fresca del oasis.

<!--/Millones de años después, en medio del desierto, los habitantes del pueblo más cercano, Pricklytown, crearon un canal subterráneo para facilitar la llegada de agua gracias a un oasis cercano. Debido a estas obras despertaron a uno de estos huevos que creó unos misteriosos cactus que empezaron a brotar sobre él absorbiendo gran parte del agua del canal. De estos cactus se obtienen unos higos chumbos jugosos e irresistibles para los vecinos de Prickytown. La abundancia de estos frutos fue muy conveniente, ya que los vecinos pasaban por un periodo de hambruna./

<img src="./Imagenes_README/HigoChumboÑam.png" alt="Higo" width="30%"/>


A medida que fueron pasando el tiempo estos cactus los habitantes de Pricklytown y alrededores empezaron a presentar un malestar profundo debido al consumo del higo chumbo. Entre los síntomas se encuentran: deshidratación general, cansancio, fiebre, enrojecimientos de la piel y/o protuberancias con forma de espinas, y en muy pocos casos diarrea. 

Debido a lo ocurrido se mandó una expedición formada por matasanos, voluntarios y miembros de la iglesia. A medida que se iban acercando a la zona cero cuando el sol ya se había ocultado, algunos expedicionistas afirmaron haber visto algunos de estos cactus moverse. 

Al día siguiente de los 20 expedicionistas originales solo volvieron 7 cargados con bolsas llenas de aquellos higos. Cuando las familias y habitantes de Pricklytown y otros pueblos cercanos pidieron explicaciones de los que había ocurrido solo dijeron que ellos eran los elegidos por los dioses. Habían visto el maravilloso poder de los higos que, según ellos, era un milagro capaz de hacer inmortales a aquellos que los consumían permitiendo una mayor conexión el mundo espiritual, llegando a formar parte de él. 

Los bendecidos con el “milagro chungo” fueron llevados de forma voluntaria o forzada a la nueva iglesia formada por aquellos miembros eclesiásticos que habían sobrevivido para realizar cultos y rituales con ellos. El “culto del higo” se expandió poco a poco hasta formar un ejército de criaturas cactus sedientas que buscaban fuentes de agua, ya fueran pozos en los pueblos o criaturas, para crecer y aumentar en número provocando disturbios en las áreas cercanas. 

Año y medio después de los acontecimientos ocurrido empieza el viaje de nuestro protagonista.--> 
 
## 2.3. Personajes principales 

### Protagonisto

El protagonista de “Proyecto Coyote”, es un vaquero que se dirige al oasis de Pricklytown para enterrar las cenizas de su difunto marido. Se trata de un humano de 40 años, robusto, con una actitud ruda. En sus brazos posee unas ruedas de revolver que sirven para lanzar sus manos como si fueran un gancho. 

<!--### Personajes no jugables (NPCs)-->

### Enemigos 

Planteamiento de siluetas

<div style="display: flex; justify-content: center; gap= 0px">
  <img src="./Imagenes_README/SiluetasCactus1.png" alt="CactusZombieSiluetas" style="width: 49%"/>
  <img src="./Imagenes_README/SiluetasCactus2.png" alt="CactusZombieSiluetas" style="width: 49%"/>
</div>

### La infección
Los pocos doctores cuerdos que han sobrevivido a la infección han observado que su síntoma más vistoso es el crecimiento de protuberancias con forma de cactus en el cuerpo de la criatura afectada. Los infectados no parecen ser conscietes de sus acciones y aparentan obedecer a una fuerza extraña.

Los estados de transformación por el “milagro chungo” se han categorizado en 4 niveles según el tiempo que lleva el individuo enfermo. 

**Fases de contagio**

<table>
  <tr>
    <th style="width: 10%;">Fase</th>
    <th style="width: 70%;">Síntomas</th>
    <th style="width: 20%; text-align: center;">Aspecto</th>
  </tr>
  <tr>
    <td>1</td>
    <td>Primeros cinco días tras el consumo del higo. En este punto, los individuos no representan anomalías corporales, únicamente fiebre. El consumidor deja de tener hambre y sed y se siente saciado.</td>
    <td><img src="./Imagenes_README/Nivel1.png" alt="Nivel 1" width="500"/></td>
  </tr>
  <tr>
    <td>2</td>
    <td>En esta fase, a partir del quinto día hasta la segunda semana, el individuo empieza a presentar protuberancias en la piel, ahora áspera en la mitad superior del cuerpo, donde los pelos adoptan una postura rígida como si fueran espinas. Además de fiebre, los sujetos presentan síntomas de deshidratación, provocando en algunos casos diarrea.</td>
    <td><img src="./Imagenes_README/Nivel2.png" alt="Nivel 2" width="500"/></td>
  </tr>
  <tr>
    <td>3</td>
    <td>Tras las dos semanas del consumo del higo las protuberancias del nivel anterior crecen de forma significativa y obteniendo forma de cactus, el individuo presenta un nivel de deshidratación extremo provocando la desaparición de sus ojos. Su piel ha perdido su color natural y los síntomas del primer nivel han desaparecido. Los sujetos pierden el sentido del habla y tienen movimientos toscos.</td>
    <td><img src="./Imagenes_README/Nivel3.png" alt="Nivel 3" width="500"/></td>
  </tr>
  <tr>
    <td>4</td>
    <td>A este punto los individuos ya se vuelven irreconocibles. Su cuerpo presenta deformaciones extremas con forma de cactus, tienen un comportamiento agresivo y cuando están en grupo parecen seguir ordenes de algún modo.</td>
    <td><img src="./Imagenes_README/Nivel4.png" alt="Nivel 4" width="500"/></td>
  </tr>
</table>


<p align = "center">
 <img src="./Imagenes_README/CactusZombie02.png" alt="CactusZombie02" width="60%"/>
 <img src="./Imagenes_README/CactusZombie01.png" alt="CactusZombie01" width="30%"/>
</p>

# 3. Jugabilidad 
Al tratarse de un videojuego en 3D y tener un combate frenético donde te enfrentas a distintos enemigos, hay que tener en cuenta como se va a mover el jugador y como se va a enfrentar a estos enemigos. 

## 3.1. Mecánicas de interacción y movimiento
El jugador se desplaza en tres dimensiones en el escenario de juego y puede realizar las siguientes acciones: 

### **Desplazamiento horizontal**
El jugador puede desplazarse en cualquier dirección horizontalmente y correr de forma limitada.
### **Desplazamiento vertical** 
El personaje tiene un **gancho** que le ayuda con la movilidad y la gestión de enemigos. Además de engancharse a zonas concretas para moverse por el mapa. El jugador puede usar el gancho para atraer enemigos hacia él o para acercarse a ellos. Esto depende del botón que pulse el jugador, es decir, si usa el gancho y se mueve hacia delante irá donde esté el gancho, mientras que si se mueve para atrás lo que tenga el gancho será atraido hacia el jugador.

Cuando se ha usado el gancho este tendrá una recarga progresiva y no se podrá usar hasta que se haya cargado de nuevo. Para hacer más rápida esta carga el jugador tendrá la posibilidad de recargar la mitad del gancho si realiza un parry a un enemigo.

El gancho cuenta con una logitud máxima de 6 metros y se desactivará la opción de usarlo con enemigos cuando se encuentre a una distancia inferios a 1,5 metros pues no se considera necesario su utilidad a esas distancias.

## 3.2. Sistema de combate y vida
### **Estilo duelo** 
El jugador puede fijar a un enemigo cuando está a cierta distancia pudiendo esquivar sus ataques (se muestra por pantalla la dirección del ataque). Si esquivas el ataque en el momento perfecto, el jugador realiza un parry bloqueando el ataque y stunneando al enemigo durante un muy corto periodo de tiempo. Habrá enemigos con ataques especiales que no se podrán esquivar o parrear obligando al jugador a desfijarlo para no recibir el daño. 

Si el jugador consigue derrotar al enemigo fijado, se enfocará automaticamente al enemigo de al lado si es que existe. 

### **Ataque** 
El jugador puede atacar en varias direcciones.

  * Izquierda: input de dirección izquierda (A/joystick) + golpear.

  * Centro: sin input de dirección + golpear.

  * Derecha: input de dirección derecha (D/joystick) + golpear.

En el caso que el jugador utilice el input de dirección adelante o atrás junto al botón de ataque se interpretará como un ataque al centro.

Si un jugador ataca en la misma dirección en la que un enemigo está bloqueando, esté realizará un contratrataque al jugador poniendolo en un aprieto.

### **Esquives**
El jugador tendrá que esquivar los ataques de los enemigos para no sufrir demasiado daño. Estos ataques se categorizan en dos tipos:

- **Ataques primarios** Son los recibidos por el enemigo fijado. Pueden venir por la derecha, izquierda o centro. Para contrarrestarlos el jugador puede realizar las siguientes acciones:

  - **Esquive**: dentro de una ventana de frames, el jugador puede esquivar en la dirección indicada por la interfaz para no sufrir daño observando 3 posibles direcciones:
    
    * Izquierda: input de dirección izquierda (A/joystick) + esquivar.

    * Centro: sin input de dirección + esquivar.

    * Derecha: input de dirección derecha (D/joystick) + esquivar.

    La dirección en la que el jugador tiene que esquivar viene indicada tanto en la interfaz como en la dirección física de la animación de ataque del enemigo. En caso de esquivar en una dirección contraria a la indicada el jugador recibirá daño. Así mismo, los enemigos también bloquearán los ataques del jugador bajo las mismas normas.

  - **Esquive perfecto**: un esquive en el momento exacto (con una ventana de frames más pequeña que el esquive normal) produce un bloqueo. Los bloqueos suponen mayor riesgo y recompensa, pudiendo dejar al enemigo aturdido si se ejecutan correctamente y pudiendo contraatacar como respuesta. Los bloqueos se realizan en las mismas direcciones que los esquives.

- **Ataques secundarios** Son los ejecutados por enemigos no fijados, que pueden atacarte por la espalda o dispararte.
  
  * Si el jugador no ha fijado ningún enemigo, puede esquivar ataques o disparos gracias a los frames de invulnerabilidad que otorga el esquive. Un esquive en el momento exacto puede ayudar a salir de la trayectoria del ataque o a omitirlo por completo si se hace correctamente.

  * Si el jugador tiene fijado a un enemigo, el resto entrarán en un estado de "Kung fu Circle", donde se turnarán para atacar al jugador de manera controlada, complicando los enfrentamientos con varios enemigos al mismo tiempo, pero haciéndolos plausibles. Si el jugador va a recibir un ataque o disparo de un enemigo no fijado mientras está en estado de combate, puede introducer el imput **esquivar + dirección atrás (s/joystick)** para realizar un esquive que le protegerá de recibir daño.

<img width="760" height="206" alt="image" src="https://github.com/user-attachments/assets/0ccea54d-8d7f-4c84-89b9-38925bfd3b2d" />


De esta forma se recompensa por realizar esquives y bloqueos correctos, fomentando esto como mecánica principal de la jugabilidad de "Proyecto Coyote".

<img width="517" height="436" alt="image" src="https://github.com/user-attachments/assets/aa28e730-dd0e-4687-9f02-7e6f53d38f46" />

### **Vida**
El jugador contará con 5 puntos de vida los cuales de pueden recargar de dos formas:

  1. **Botiquines**: En los niveles el jugador podrá encontrar botiquines en zonas de transición entre combates. Estos botiquines restauran 2 puntos de vida.

  2. **Recuperar vida**: Cuando recibes un ataque de un enemigo ya sea en el estilo duelo o no, los corazones quedan quitados correspondiendo al daño inflijido. Si el jugador realiza un esquive perfecto durante un combate fijado podrá recuperar la vida que le fue quitada. Si el jugador vuelve a recibir un ataque y no ha podido recuperar los corazones quitados, esos corazones desaparecerán y los corazones quitados pasarán a ser los inflijidos por el último ataque. Si un enemigo realiza un ataque que tiene más daño que corazones porta el jugador este morirá automaticamente.

  <img src="./Imagenes_README/DiagramaVida.png" alt="DiagramaVida" style="width: 70%"/>

### **Comportamiento enemigos**

El comportamiento de los enemigos varía dependiendo del lo que haga el jugador:

  * **Estado ilde**: Cuando los enemigos no detectan al jugador al estar fuera de su rango de visión u oculto, estarán en estado ilde dando vueltas o quedandose quietos realizando una animación predefinida. A este estado también volverán los enemigos cuando el jugador se aleje lo suficiente de estos tras ser detectado probocando que lo dejen de perseguir.

  * **Estado combate**: Si el jugador es detectado por un enemigo este le atacará a distancia o se acercará para pegarle. Si otros enemigos ven a uno de ellos ponerse en este modo buscarán al jugador para atacarle también.

## 3.3. Controles 

MECÁNICA              | TECLADO     | Dispositivos táctiles
--                    | --          | --
MOVIMIENTO DE CÁMARA  | RATÓN       | Joystick tactil der.
MOVIMIENTO            | W,A,S,D     | Joystick tactil izq.
CORRER                | SHIFT       | 
GANCHO                | E           |
ATAQUE PRINCIPAL      | CLICK IZQ.  |
ESQUIVE/BLOQUEO         | ESPACIO     |

## 3.4. Tipos de enemigos 

### Enemigos melee 
Los enemigos melee como su nombre indica atacan a corta distancia. Son resistentes a los ataques y no poseen armas a distancia con las que atacar. Este tipo de enemigo tiene un rango de detección de 7 metros y pega con sus puchos. Tiene un total de 3 variantes, cada una correspondiente a un bioma.

#### Melee Básico
Este enimgo sirbe de seudotutorial al ser un enemigo sin ningún tipo de combos. Su aparición será principalmente en el primer nivel y cuenta con ataques y bloqueos básicos.

#### Melee Pricklytown
Este enemigo se encuentra por el pueblo de Pricklytown. Sus ataques hacen 1 de daño y cuenta con 4 puntos de vida.  Posee un ataque (“ataque abrazo”) que solo se puede esquivar por el centro y si el jugador lo recibe se queda aturdido por unos segundos quedando expuesto a cualquier ataque. Sus patrones de ataque son:
  1. Bloque izquierdo y central, ataque central.
  2. Bloque izquierdo y central, ataque central y ataque izquierdo.
  3. Bloqueo total y "ataque abrazo".
  4. Ataque derecho, bloqueo derecho y ataque central

#### Melee Cañón
Este enemigo se encuentra en la zona del cañón. Sus ataques hacen 1 de daño y cuenta con 4 puntos de vida. Al igual que la variante anterior posee un ataque (“ataque abrazo”) que solo se puede esquivar por el centro y si el jugador lo recibe se queda aturdido por unos segundos quedando expuesto a cualquier ataque. Sus patrones de ataque son:

  1. Bloqueo total y "ataque abrazo".
  2. Ataque derecho, ataque derecho y ataque izquierdo.
  3. Ataque derecho, ataque derecho, bloqueo derecho y ataque izquierdo.

#### Melee Oasis

ste enemigo se encuentra en la zona del oasis. Sus ataques hacen 1 de daño y cuenta con 5 puntos de vida.Sus patrones de ataque son:

  1. Ataque derecho, ataque derecho y ataque izquierdo.
  2. Ataque central, bloqueo central y ataque derecho.
  3. Bloqueo izquierdo, ataque derecho y bloqueo derecho.

### Enemigo suicida
Este enemigo busca provocar el mayor daño posible al jugador. Cuando entra en su campo de visión (7 metros) va corriendo tras él y, cuando se encuentra a una distancia inferior a un metro, se lanza hacia el jugador creando una explosión que acaba con su vida y provoca 3 de daño si no se esquiva. Si el jugador usa el gancho contra él, el enemigo explotará cuando el jugador lo atraiga o vaya hacia él.

Para poderse librar de este enemigo existen varias opciones:

  * Cuando el jugador se mueve por el escenario, si realiza un esquive en el momento justo no recibirá ningún daño.

  * Si el jugador fija a este enemigo, este realizará un ataque suicida en una de las tres direcciones de forma aleatoria. Como este ataque no es esquivable el jugador tendrá que atacar al enemigo antes de que le ataque para lanzarlo y que explote.

  * Si el jugador usa el gancho contra el enemigo cuando se acerta tiene una pequeña ventana para atacarlo y lanzarlo provocando su explosión.

### Enemigo robusto
El enemigo robusto es un tipo de enemigo con mucha resistencia a los golpes, contando de 7 puntos de vida. Cuando usas el gancho con este enemigo solo puedes ir hacia él, en el caso que quieras atraerlo hacia a ti el gancho volverá solo. Cuando está a cierta distancia del jugador el enemigo lanzará piedras hacia su dirección, que causarán aturdimiento y uno de daño si no se esquiva.En ocasiones podrá lanzar a los enemigos suicidas. Cuando el jugador está cerca o lo tiene enfocado realizará ataques más lentos de lo normal, pero hacen 2 de daño. Este enemigo cuenta con los siguientes patrones de ataque:
 
  1.	Ataque central, defensa central y ataque central.
  2.	Defensa izquierda, ataque izquierdo y ataque derecho.
  3.	Ataque izquierdo (con amague de central), ataque derecho y defensa derecha.

### Francotirador
Como su nombre indica este enemigo porta un arma con forma de francotirador, pudiendo detectar al jugador en un rango de 14 metros. Sus disparos tienen un daño de 2 corazones y tarda unos 3 segundo en poder volver a disparar.Para poder disparar se tendrá que cubrir por una cobertura. Este enemigo cuenta con 3 puntos de vida. Cuando el jugador enfoca a este enemigo adopta una postura defensiva cubriéndose por dos lados a la vez. Cuenta con 5 patrones de defensa:
  1. Defensa izquierda.
  2. Defensa izquierda y defensa central.
  3. Defensa central e izquierda a la vez.
  4. Defensa derecha.
  5. efensa central e izquierda a la vez y defensa derecha.

Si el jugador golpea en la dirección donde defiende, este huirá a la cobertura más cercana. En caso de que el jugador enfoque a otro enemigo o reciba el ataque de otro por detras este también huirá a la cobertura más cercana.


# 4. Arte 

A continuación se hará un resumen del apartado artístico general de Proyecto Coyote, desde el arte conceptual, inspiraciones y paletas de color hasta el arte final que se utilice en el juego. Para información más detallada sobre guías de diseño y modelado, procesos de trabajo y especificaciones artísticas, consultar el **documento de estilo**.

## 4.1. Estilo artístico general

Como ya se ha mencionado el juego estará completamente implementado en 3D, tanto escenarios como personajes. Por una combinación de necesidades técnicas y decisiones artísticas, los 

## 4.2. Personajes

###  Personaje principal

<img src="./Imagenes_README/MainChar1.jpg" alt="Prota1" style="width: 70%"/>

Primer concept art del protagonista. Proyecto de ficha de personaje

<img src="./Imagenes_README/MainChar2.jpg" alt="Prota2" style="width: 70%"/>

Concept art a color. Diseño no final.

<img src="./Imagenes_README/MainChar3.jpg" alt="Prota3" style="width: 70%"/>

Concept art con el diseño final del personaje

<img src="./Imagenes_README/MainChar4.jpg" alt="Prota3" style="width: 70%"/>

Modelo 3D finalizado del personaje principal

## 4.3. Escenarios

Como se ha mencionado anteriormente, "Proyecto Coyote" cuenta con tres zonas principales,cada una de estas zonas será un único nivel por lo tanto, el juego contará con tres niveles bien diferenciados. Para la introducción y la transición de niveles se meterán cinemáticas simulando ser un comic.

### Pueblo

![alt text](<Nivel1Con elementos del nivel.png>)

Este es el mapa del primer nivel del juego que se desarrolla en el pueblo de Pricklytown. El nivel esta dividido en 5 subáreas de combate donde también podrá encontrarse con distintos Npcs que le pondrá en contexto sobre que son esos seres con forma de cactus.
### Cañón

![alt text](<Nivel2Con elementos del nivel.png>)

El segundo nivel se sitúa en un oásis. En el mapa se puede observar que es un nivel alargado y dividido en 4 subáreas de combate.
### Oasis

![alt text](<Nivel3Con elementos del nivel.png>)

El último nivel se desarrolla en el oasis donde se encuentra tanto el huevo cosmico como la iglesia de la secta. Este nivel es más corto de los demás dividiendolo en 3 áreas de combate. Las 2 primeras son áreas que se dividen en 2 oleadas y la última zona es donde se desarrolla la batalla contra el jefe final.

###  Enemigos
<img src="./Imagenes_README/ConceptZombie.JPG" alt="zombi1" style="width: 36%"/>
<img src="./Imagenes_README/ConceptEsqueleto.jpg" alt="zombi1" style="width: 59%"/>

Arte conceptual inicial de algunos enemigos

<img src="./Imagenes_README/ZombieNopalAttackFront.JPG" alt="zombi1" style="width: 30%"/>
<img src="./Imagenes_README/ZombieNopalIdle.JPG" alt="zombi2" style="width: 31%"/>
<img src="./Imagenes_README/ZombieNopalWalk.JPG" alt="zombi3" style="width: 24%"/>

Modelado de el Zombi Nopal

## 4.4. Arte 2D



# 5. Sonido y música 
## 5.1. Estilo Sonoro y musical
La música será ambientada en bandas sonoras del oeste para el menú de introducción, y temas de acción y aventura para el gameplay. Se usará el software “Musescore” para la composición de las canciones. Algunos instrumentos empleados serán el banjo o la trompeta entre otros.

Por otra parte, los sonidos y efectos especiales (SFX) tendrán una temática 8 bits, para acompañar la estética visual del juego. Se emplearán páginas como SFXR o BFXR.

<!--## 6.2. Banda sonora 
## 6.3. Efectos sonoros (SFX) -->

# 6. Menús e Interfaces 
## 6.1. Diagramas de flujo 

### Menú inicio
<img width="1050" height="562" alt="DiagramaflujoMenuPrincipal drawio" src="https://github.com/user-attachments/assets/b133867e-11f8-44f8-8a3e-f341eae1f700" />

### Menú opciones
<img width="611" height="301" alt="DiagramaflujoMenuOpciones drawio" src="https://github.com/user-attachments/assets/f8e94939-37c0-43d9-bf1f-1a1998928cb2" />

### In game
<img width="716" height="862" alt="DiagramaflujoInGame drawio" src="https://github.com/user-attachments/assets/551992dc-3eae-4f19-a586-60a36025282a" />

### Requisitos funcionales
__Menú inicio:__ El menú de inicio es lo primero que se encuentra el jugador cuando inicia el juego. En este menú se pueden observar los siguientes botones:
 
 * __Nueva partida:__ El jugador comienza el juego desde 0.
 
 * __Continuar:__ El jugador continua la partida desde el punto donde lo había dejado en caso de tener una partida guardada, si no la tiene no pasará nada.
 
 * __Controles:__ El jugador podrá ver los controles tanto en ordenador como dispositivos móviles.
 
 * __Opciones:__ El jugador accederá al menú de opciones donde podrá ajustar los niveles de audio general, música y efectos sonoros.
 
 * __Créditos:__ El jugador accede a la pantalla de créditos donde aparecerá los miembros que conforman el equipo y su trabajo realizado.
 
 * __Contenido descargable:__ Al pulsar este botón se desplegará una pantalla donde se podrá observar el contenido descargable de pago que contiene le juego.
 
 * __Salir:__ Con este botón el jugador saldrá del juego.

__In game:__ Para acceder al menú de pausa es tendrá que pulsar la tecla "esc" en ordenador o el respectivo botón en dispositivos móviles.

 * __Menú pausa:__ En este menú se presentan tres opciones al jugador:
   
    1. __Salir:__ permite al jugador volver al menú inicial.
       
    3. __Reintentar:__ resetea el nivel volviendo a iniciar desde la cinemática.
       
    5. __Reanudar:__ vuelve al nivel en el momento que lo pausó.

 * __Muere:__ Cuando el personaje muere se presenta ante él una pantalla de Game Over con dos opciones:
   
    1. __Reintentar:__ vuelve al último Check point.
       
    3. __Salir:__ vuelve al menú inicial.
       
 * __Termina el nivel:__ Al completar el nivel se presentan dos situaciones. Si hay otro nivel después se pasará al siguiente, pero si ya ha terminado el juego irá a la pantalla de créditos y luego al menú inicio cuando acabe.

 ## 6.2. Diseño de interfaces
 ###  Menú principal

<img src="./Imagenes_README/BocetoMenuPrincipal.png" alt="BocetoMenuPincipal" style="width: 70%"/>

1.	Título del juego.

2.	Botones con las distintas opciones siguiendo el estilo del título.

3.	De fondo se ve el pueblo de Prinklytown.

4. El protagonista está de espaldas a la cámara mirando hacia el pueblo mientras se le mueve el poncho.

# 7. Modelo de Negocio y Monetización

## 7.1. Monetización

Al tratarse de un videojuego de acción frenética en tercera persona, el tipo de monetización que más encaja con nuestro tipo de juego es ***Buy to Play***. 

Pese a ser de pago único, se pondrá a la venta un **early access** para los jugadores que quieran jugar el juego aunque no esté terminado, se habilitará la opción de hacer **pedidos anticipados** y se abre la puerta a la posibilidad de comercializar **DLCs** en función del éxito que tenga el juego.

PRODUCTO              |  PRECIO
--                    |  --     
Juego base  |   20 €
DLC: libro de arte |   7.5 €

## 7.2. Planificación y Costes
### El equipo humano
  * __Antonio__:	Diseñador de sonido y programador.
  * __Diego__:	Programador.
  * __Candela__:	Artista 2D, artista 3D y animadora 3D.
  * __Edu__:		Artista 2D, artista 3D, animador 3D y artista técnico.
  * __Andrea__:	Programadora.
  * __David__:	Game designer y guionista.

### Estimación temporal del desarrollo

La realización del proyecto tendrá un tiempo límite de 13 semanas teniendo en cuenta una subdivisión en distintas etapas:

  *	Versión Alpha (semana 5): Para esta versión del juego se tendrá programada una versión simple del combate, donde habrá un jugador y un enemigo que entra en combate. Esta versión contará con un tutorial para que se entienda bien las mecánicas principales.

  * Versión Beta (semana 9): Para esta versión se tendrán montados los niveles, enemigos y programado y mejorado mecánicas como el gancho, el combate o la cámara. Se intentará tener texturizado la gran mayoría de los elementos del juego, igual que un sistema de interfaces y menús funcionales e intuitivos.

  * Versión Release (semana 13): Esta es la versión final del juego. Aquí se mostrará el juego terminado habiendo pasado por un testeo, corrección de errores y terminado de añadir detalles menores del mismo.

Durante estas versiones cada semana se pondrán metas a alcanzar para el final de esta.

### Costes adicionales

#### Costes del personal
Estos serían los costes directos de la empresa, The Big Bone Team, asumiendo que el proyecto va a durar 13 semanas:

MIEMBRO DEL EQUIPO  |  ROL | COSTE POR HORA (€) | COSTE MENSUAL (€) | COSTE TOTAL (€)
--  |  -- |  -- |  --  |  --  
Andrea |   Programadora | 20 | 2400 | 7800
Antonio | Diseñador de sonido y programador | 25 | 3000 | 10050
Candela | Artista 2D, artista 3D y animadora 3D | 15 | 1800 | 5850
Diego | Programador | 25 | 3000 | 10050
David | Game designer y guionista | 15 | 1800 | 5850
Edu | Artista 2D, artista 3D y animador 3D | 15 | 1800 | 5850
__Total__ | - | 85 | 10200 | 45450

#### Licencias y software
CONCEPTO | COSTE MENSUAL (€) | COSTE TOTAL (€)
-- | -- | --
Internet | 60 | 195
Unity: Licencia Pro | 925 | 3006.25
Procreate | (coste único) | 14.99
Clip Studio | (coste único) | 49
Github | Licencia gratuita | 0
Aseprite | Licencia gratuita| 0
Miro | Licencia gratuita | 0
Jira | Licencia gratuita| 0
Mocrosoft Teams | Licencia gratuita| 0
Blender | Licencia gratuita | 0
MuseScore | Licencia gratuita | 0
Google Docs | Licencia gratuita | 195
__Total__ | 985 | 3265.34

#### Otros gastos
CONCEPTO | COSTE MENSUAL (€) | COSTE TOTAL (€)
-- | -- | --
Alquiler oficina | 800 | 2600
Internet | 60 | 195
Equipos | (coste único) | 5000
Servicios externos (testers) | 500 | 3000
__Total__ | 2285 | 10795

# 8. Marketing y Redes Sociales

Para publicitar el juego se han creado distintas redes sociales como Instagram, Youtube, X, ... El uso de estas redes sociales serán para presentar los distintos integrantes del equipo y los pequeños avances que se hagan conforme avanza el tiempo.

El público objetivo de este proyecto son personas adolescentes mayores de 16 años con interés y algo de experiencia en videojuegos de combate frenético en tercera persona, que sientan atracción por los mundos de fantasía, el viejo oeste o por la estética lovecraftiana, por lo que habrá que enfocar el marketing hacia dicho público.

# 9. Post Mortem

## 9.1. Post Mortem - Alfa

### Eduardo Almarza Blasco

#### Lecciones aprendidas

La necesidad de estructurar las tareas del proyecto desde el minuto uno, evitando dependencias y esperas y, sobre todo, asumiendo fechas que permitan una cierta holgura en caso de que las cosas se retrasen. Necesitamos reducir el scope ligeramente, pero sobre todo crear un "roadmap" concreto donde nos establezcamos en qué fechas deben estar hechas cada una de las tareas. Comunicarnos para estar todos en una misma página antes de empezar a trabajar es crucial para no hacer trabajo innecesario o duplicado.

Por ejemplo, cuando los artistas del proyecto empezamos a desarrollar concept art, personalmente salté demasiado pronto al diseño del protagonista. Esto complicó adaptarlo a las siguientes iteraciones de la idea y del trabajo, cosa que hubiese sido más sencilla si me hubiera esperado a tener la idea del juego más madura.

#### Trabajo individual realizado

Por mi parte, mi principal tarea ha sido realizar el diseño, arte conceptual, modelo 3D, texturizado rigging y animación del personaje principal, tanto en Blender como en la integración al motor, habiendo programado el animator controller del mismo.

También he realizado trabajos menores en el apartado visual del Alpha, como el Shader del personaje (que se utilizará después para todos los personajes), la UI, la integración de las animaciones en el motor y ligeros efectos de fedback.

Finalmente, y ya fuera de lo que es el juego, he realizado el logo del grupo y he comenzado a definir lo que será la estética de nuestras redes sociales (fuentes, estilos, paletas de color...).

#### Trabajo colectivo realizado

He trabajado de manera continua con Candela en el planteamiento y desarrollo de todo el apartado artístico. Desde las primeras reuniones de proyecto, hemos decidido entre los dos el estilo visual, los detalles de la ambientación y las especificaciones concretas de los modelos. Además nos hemos encargado de definir un *workflow* común para trabajar los personajes y su importación desde Blender.

También me he mantenido en contacto con los programadores para comprender sus sistemas y poder modificarlos si fuese necesario en mi labor de artista técnico. Como ya he mencionado me he encargado de modificar su código para, por ejemplo, añadir efectos de animación o componer los animator controllers.

Además he tenido un papel significativo en la toma de decisiones sobre diseño, ambientación y, en general, sobre la idea del juego en las primeras reuiniones

---

### Antonio Bernal de Celis
#### Lecciones aprendidas

La importancia de trabajar en equipo, de tener unos propósitos marcados claramente desde el primer momento y fijar unas fechas para cumplir estos objetivos. También he aprendido la importancia de no tener miedo de recurrir a compañeros por ayuda.

#### Trabajo individual realizado

Mi trabajo individual ha sido la creación de un sistema de movimiento para el personaje controlable. En este sistema, el personaje es capaz de andar, correr y realizar un dash como movimientos básicos. También se le ha aplicado gravedad al personaje y se ha empezado el sistema de gancho para poder atraer/acercarse a enemigos.

He implementado el input system para que controlar las entradas del jugador y configurar los controles en función del dispositivo con el que se esté jugando (teclado, móvil o mando). Se ha empezado a configurar los controles del mando.

#### Trabajo colectivo realizado

He mantenido contacto sobre todo con el equipo de programación (Andrea, Diego y yo). Nos hemos comunicado las actualizaciones de cada uno. Mientras yo programaba el movimiento, Andrea hacía la cámara y estábamos en constante contacto.

Además, todo el trabajo de programación ha pasado por la supervisión de Diego y también he estado en contacto con él como asesor.

---

### David del Castillo Enríquez
#### Lecciones aprendidas

En cuanto a las lecciones aprendidas me he dado cuenta de que es muy importante tener una visión clara del proyecto desde el principio, pues en distintos momentos de esta versión nos hemos dado cuenta de que teníamos visiones algo distintas tanto en el planteamiento de ciertos aspectos del gameplay como identidad del juego

#### Trabajo individual realizado

Mi trabajo individual realizado ha sido principalmente crear el linktree y la mayoría de las redes sociales, pensar en los escenarios que tendrá el juego, los distintos enemigos y comportamientos de estos, aclarar y rellenar apartados de la jugabilidad dentro del GDD para que todos los miembros estén de acuerdo, los diagramas de flujo junto a su explicación, la implementación de un boceto de idea del menú principal y la creación básica del menú de juego.

En este trabajo la verdad que he sentido libertad a lo que respecta a mi apartado como game designer obviamente planteando de antemano ciertas ideas y conceptos a mis compañeros para saber su opinión.

Es verdad que tengo la sensación de que podría haber avanzado más o planteado cosas más pronto en cuanto a mi trabajo dentro del grupo. 

#### Trabajo colectivo realizado

Mi trabajo colectivo con otros miembros del equipo ha sido con Candela a la hora de plantear la historia y rellenar lo esencial para el GDD.
La comunicación con mi compañera a la hora de plantear los puntos principales de la historia y el Wordbuilding ha sido muy buena.

En general yo planteaba los conceptos o ideas en el GDD con un desarrollo y ella los revisaba y corregía reescribiéndolo para darme cuenta de la visión general y no alejarme de ella, además de darle un toque más narrativo.

También cabe resaltar que la idea de que el prota tenga que enterrar las cenizas de su marido fue una idea general del grupo al igual que la ambientación. 

---
### Diego Fernández Manso

#### Lecciones aprendidas
  Principalmente se ha aprendido sobre división de trabajo de cara a programación. Eramos un grupo de tres centrados en programación y hemos tenido que dividir un proyecto relativamente grande.
  En relacion al propio desarrollo, se ha aprendido mucho sobre inteligencia artifiical y comprotmaiento de personajes ya que se ha utilizado el API de arboles de comportamiento de personajes.

#### Trabajo individual realizado
  El trabajo individual se ha centrado en mayor parte con el sistema de combate y el comportamiento de los enemigos.
  El mayor pro es que al ser una parte relativamente aislada del resto del proyecto se ha podido trabajar sin muchos conflictos con el trabajo del resto del equipo. Con la excepción de sistemas como los controles que por suerte estaban pensados desde el principio para una buena integración con el sistema de combate.
  En lo negativo, al ser muchos sistemas entrelazados (IA, combate, vida, llamadas a animators) ha habido muchos días donde el trabajo daba pocos o ningun resultado. 
  Ademas se han tenido que rehacer varias veces los sistemas ya que se habían diseñado sin tener en cuenta todas las necesidades de diseño.

#### Trabajo colectivo realizado
  El mayor problema colectivo a nivel de programación es que algunas tareas no se han dividido muy bien, lo que ha llevado a cierta solapación en los sistemas a realizar, lo que ha significado trabajo perdido.
  En el lado positivo, una vez se corrigieron estos problemas iniciales hemos podido sincronizarnos bien y, debido a que tenemos experiencia previa con github, hemos podido evitar conflictos de archivos y branches.

---

### Candela Jiménez González
#### Lecciones aprendidas
  La entrega de la versión alfa ha sido clave para considerar de forma más realista el scope del proyecto completo. Ver hasta dónde hemos llegado como equipo en esta primera entrega nos ha permitido entender que el primer planteamiento del proyecto se nos iba un poco de las manos y que iba a ser necesario el uso de herramientas de gestión y la división de tareas en subtareas con fechas de realización. Ha habido varios objetivos que planteamos al principio a los que no se han podido llegar por esta falta deorganización. 

#### Trabajo individual realizado
  Mi trabajo principal en esta entrega ha consistido en el diseño, modelado y animación del enemigo base, creando sus animaciones de ataque, de bloqueo y de caminar. 

  Además, me he encargado de realizar concept art (tandto de enemigos como de personajes no jugables) a partir del trasfondo del juego que hemos ido construyendo. He participado mucho también precisamente en el desarrollo de esta historia y en el worlbuilding general. 

  También he sido la encargada de la estructuración y la escritura de gran parte del Readme, así como de la creación de algunos de los recursos visuales de este.

  Finalmente, también he empezado con el diseño y planteamiento inicial de la página web.
  
#### Trabajo colectivo realizado
  He trabajado en todo el desarrollo de esta entrega con Edu, sobre todo cuadrando el estilo visual general y tomando decisiones las de diseño que más nos cuadrasen conjuntamente para el proyecto. Nos hemos encargado de precisar el concepto de personajes y escenarios para que, ya que ambos vamos a trabajar en arte, partamos de un punto común y tener clara la estética y la sensación que queremos darle al juego. Esto ha sido fácil gracias a la creación de un documento de estilo y gracias a nuestra continua comunicación durante el desarrollo de assets.
  
  También he colaborado conjuntamente con Diego y Edu a la hora de integrar las animaciones en el motor de desarrollo e incluirlas en el controlador. 

  ---

### Andrea Luengo Zazo
#### Lecciones aprendidas
  En el ámbito teórico se ha aprendido sobre monetización y gestión de proyectos. Por otro lado, en el desarrollo del proyecto se ha ampliado en gran cantidad el conocimiento relacionado con *cinemachine* y cámaras virtuales, en concreto de *state-driven camera* para cambiar de una cámara a otra a través de un *Animator*. También se ha aprendido a localizar enemigos en un rango concreto y se ha aumentado la comprensión del espacio 3D en Unity.
#### Trabajo individual realizado
  El trabajo individual realizado a lo largo de la primera fase del proyecto se ha centrado sobre todo en el desarrollo del sistema de cámaras, la transición entre los diferentes modos(Lock y libre) y la detección de los enemigos en un rango concreto.
#### Trabajo colectivo realizado
  Gestión y reparto de tareas.

<!--## 9.2. Post Mortem - Beta
## 9.3. Post Mortem - Gold-->
