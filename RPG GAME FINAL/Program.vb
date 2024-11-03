Imports System
Imports System.Collections.Generic

Module DungeonExplorer

    ' Player properties
    Private playerHealth As Integer = 100
    Private playerAttack As Integer = 10
    Private playerDefense As Integer = 5
    Private playerInventory As New List(Of String)
    Private monsterCount As Integer = 0
    Private level As Integer = 1
    Private currentWeapon As String = "Basic Sword"
    Private weaponType As String = "Sword"
    Private elementType As String = "None"

    Private random As New Random()

    Sub Main()
        Console.WriteLine("Welcome to Dungeon Explorer!")
        Console.WriteLine("Survive as long as you can by defeating monsters and bosses.")
        Console.WriteLine()

        Dim isPlaying As Boolean = True
        While isPlaying
            Console.Clear()
            ShowPlayerStats()
            Console.WriteLine($"You are on Level {level}.")
            Dim action As String = ChooseAction()

            Select Case action
                Case "1"
                    If monsterCount > 0 AndAlso monsterCount Mod 5 = 0 Then
                        Console.WriteLine("A powerful boss stands in your way!")
                        EncounterBoss()
                    Else
                        EncounterMonster()
                    End If
                Case "2"
                    SearchForItem()
                Case "3"
                    isPlaying = False
            End Select

            If playerHealth <= 0 Then
                Console.WriteLine("You have died. Game Over!")
                isPlaying = False
            End If
        End While

        Console.WriteLine("Thank you for playing Dungeon Explorer!")
        Console.ReadLine()
    End Sub

    Private Sub ShowPlayerStats()
        Console.WriteLine($"Health: {playerHealth} | Attack: {playerAttack} | Defense: {playerDefense}")
        Console.WriteLine($"Current Weapon: {currentWeapon} (Type: {weaponType}, Element: {elementType})")
        Console.WriteLine("Inventory: " & If(playerInventory.Count > 0, String.Join(", ", playerInventory), "Empty"))
    End Sub

    Private Function ChooseAction() As String
        Console.WriteLine("Choose an action:")
        Console.WriteLine("1. Explore further into the dungeon")
        Console.WriteLine("2. Search for items")
        Console.WriteLine("3. Exit game")
        Console.Write("Enter your choice: ")
        Return Console.ReadLine()
    End Function

    Private Sub EncounterMonster()
        monsterCount += 1
        Dim monsterHealth As Integer = random.Next(20 + level * 2, 50 + level * 3)
        Dim monsterAttack As Integer = random.Next(5 + level, 15 + level)
        Dim monsterType As String = GenerateMonsterType()
        Dim monsterName As String = $"{monsterType} Monster"

        Console.WriteLine($"A {monsterType} {monsterName} appears with {monsterHealth} health and {monsterAttack} attack power!")
        While monsterHealth > 0 AndAlso playerHealth > 0
            Console.WriteLine("1. Attack")
            Console.WriteLine("2. Try to Escape")
            Console.Write("Choose your action: ")
            Dim choice As String = Console.ReadLine()

            Select Case choice
                Case "1"
                    Dim playerDamage As Integer = CalculateDamage(monsterType)
                    monsterHealth -= playerDamage
                    Console.WriteLine($"You deal {playerDamage} damage to the {monsterName}!")

                    If monsterHealth > 0 Then
                        Dim monsterDamage As Integer = Math.Max(monsterAttack - playerDefense, 1)
                        playerHealth -= monsterDamage
                        Console.WriteLine($"The {monsterName} attacks you for {monsterDamage} damage!")
                    Else
                        Console.WriteLine($"You defeated the {monsterName}!")
                        If monsterCount Mod 5 = 0 Then
                            level += 1
                        End If
                    End If

                Case "2"
                    If random.Next(0, 2) = 0 Then
                        Console.WriteLine("You successfully escaped!")
                        Return
                    Else
                        Console.WriteLine("Escape failed!")
                        Dim monsterDamage As Integer = Math.Max(monsterAttack - playerDefense, 1)
                        playerHealth -= monsterDamage
                        Console.WriteLine($"The {monsterName} attacks you for {monsterDamage} damage!")
                    End If

                Case Else
                    Console.WriteLine("Invalid choice. The monster attacks you!")
                    Dim monsterDamage As Integer = Math.Max(monsterAttack - playerDefense, 1)
                    playerHealth -= monsterDamage
            End Select
        End While
    End Sub

    Private Sub EncounterBoss()
        Dim bossHealth As Integer = random.Next(100 + level * 10, 150 + level * 12)
        Dim bossAttack As Integer = random.Next(20 + level * 2, 30 + level * 3)
        Dim bossName As String = "Dungeon Boss"

        Console.WriteLine($"A mighty {bossName} appears with {bossHealth} health and {bossAttack} attack power!")
        While bossHealth > 0 AndAlso playerHealth > 0
            Console.WriteLine("1. Attack")
            Console.WriteLine("2. Try to Escape")
            Console.Write("Choose your action: ")
            Dim choice As String = Console.ReadLine()

            Select Case choice
                Case "1"
                    Dim playerDamage As Integer = Math.Max(playerAttack - random.Next(0, 5), 1)
                    bossHealth -= playerDamage
                    Console.WriteLine($"You deal {playerDamage} damage to the {bossName}!")

                    If bossHealth > 0 Then
                        Dim bossDamage As Integer = Math.Max(bossAttack - playerDefense, 1)
                        playerHealth -= bossDamage
                        Console.WriteLine($"The {bossName} attacks you for {bossDamage} damage!")
                    Else
                        Console.WriteLine($"You defeated the {bossName}!")
                        level += 1
                        ChooseWeaponReward()
                    End If

                Case "2"
                    If random.Next(0, 2) = 0 Then
                        Console.WriteLine("You successfully escaped!")
                        Return
                    Else
                        Console.WriteLine("Escape failed!")
                        Dim bossDamage As Integer = Math.Max(bossAttack - playerDefense, 1)
                        playerHealth -= bossDamage
                        Console.WriteLine($"The {bossName} attacks you for {bossDamage} damage!")
                    End If

                Case Else
                    Console.WriteLine("Invalid choice. The boss attacks you!")
                    Dim bossDamage As Integer = Math.Max(bossAttack - playerDefense, 1)
                    playerHealth -= bossDamage
            End Select
        End While
    End Sub

    Private Sub ChooseWeaponReward()
        Console.WriteLine("Choose your reward:")
        Console.WriteLine("1. Sword/Axe")
        Console.WriteLine("2. Bow/Crossbow")
        Console.WriteLine("3. Magic (Random element: Fire, Water, or Air)")
        Console.Write("Enter your choice: ")
        Dim choice As String = Console.ReadLine()

        Select Case choice
            Case "1"
                weaponType = "Sword"
                playerAttack += 5
                currentWeapon = "Sword/Axe"
            Case "2"
                weaponType = "Bow"
                playerAttack += 4
                currentWeapon = "Bow/Crossbow"
            Case "3"
                weaponType = "Magic"
                elementType = GetRandomElement()
                playerAttack += 3
                currentWeapon = $"{elementType} Magic"
        End Select
        Console.WriteLine($"You equipped a new weapon: {currentWeapon}")
    End Sub

    Private Function GetRandomElement() As String
        Dim elements As String() = {"Fire", "Water", "Air"}
        Return elements(random.Next(elements.Length))
    End Function

    Private Function GenerateMonsterType() As String
        Dim types As String() = {"Fire", "Water", "Air"}
        Return types(random.Next(types.Length))
    End Function

    Private Function CalculateDamage(monsterType As String) As Integer
        Dim baseDamage As Integer = Math.Max(playerAttack - random.Next(0, 5), 1)
        If weaponType = "Magic" Then
            If (elementType = "Water" AndAlso monsterType = "Fire") OrElse
               (elementType = "Fire" AndAlso monsterType = "Air") OrElse
               (elementType = "Air" AndAlso monsterType = "Water") Then
                baseDamage *= 2
                Console.WriteLine("It's super effective!")
            End If
        End If
        Return baseDamage
    End Function

    Private Sub SearchForItem()
        Dim items As String() = {"Health Potion", "Sword", "Shield", "Mystery Potion"}
        Dim item As String = items(random.Next(items.Length))
        Console.WriteLine($"You found a {item}!")

        Select Case item
            Case "Health Potion"
                playerHealth = Math.Min(playerHealth + 20, 100)
                Console.WriteLine("You drink the Health Potion and restore 20 health.")
            Case "Sword"
                playerAttack += 5
                Console.WriteLine("You equip the Sword, increasing your attack power by 5.")
            Case "Shield"
                playerDefense += 3
                Console.WriteLine("You equip the Shield, increasing your defense by 3.")
            Case "Mystery Potion"
                Dim effect As Integer = random.Next(-10, 20)
                playerHealth = Math.Min(playerHealth + effect, 100)
                Console.WriteLine($"You drink the Mystery Potion and gain {effect} health.")
        End Select

        playerInventory.Add(item)
        Console.WriteLine("Press Enter to continue...")
        Console.ReadLine()
    End Sub

End Module
