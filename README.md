# GD3_LoopHero_Unity

Ce qui a été réalisé pour le rendu du 07/04/2026 :

Ajout d’un mini-jeu sur la case devant le portail. Celui-ci a pour objectif de récupérer les 3 clés cachées dans la ferme de Thomas afin de pouvoir ouvrir le portail et passer à la suite du plateau.

Thomas représente ici une IA disposant de plusieurs comportements intégrés :

  - Il patrouille dans sa ferme de point en point, suivant une boucle.
  - Il chasse, une fois le joueur détecté, il lui court après jusqu’à être suffisamment proche pour lui asséner un coup de poing. Si le joueur est touché, il est téléporté au début du niveau et doit recommencer la collecte des clés.
  - De temps en temps, il s’arrête durant sa patrouille pour observer autour de lui.

Un level design a été mis en place en restant fidèle à ce qui avait été fait précédemment. Des ajustements de dialogues ont également été réalisés, principalement lors de l’arrivée sur la case devant le portail, qui déclenche le mini-jeu. Un visuel (PNG) informe le joueur de sa mission dans le mini-jeu ainsi que des dangers possibles. Concernant l’UI, un compteur de clés a été ajouté afin d’améliorer la lisibilité de l’objectif.

Le personnage que l'on incarne dans le miniJeu possède des animations de déplacement basique. Plus l'ajout d'une animation quand le personnage récupére une clé.

Ce qui a été réalisé pour le rendu du 27/04/2026 :

Ajout d'un mini jeu sur la case de la collecte de bois. A la manière du mini jeu créer sur Unreal, on doit cliquer sur l'arbre dans le temps imparti pour faire tomber des buches de bois une fois 20 collectées le mini jeu prend fin sauf si le timer arrive à 0 cela fait redémarrer le mini jeu. 
Un environnement à été mis en place pour le faire correspondre à la DA du projet. Ainsi que l'ajout d'une UI pour comprendre ce qu'il faut faire. 
