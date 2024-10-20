/** 
 * <author>Christophe Roblin & Warped Ibun</author>
 * <email>lifxmod@gmail.com</email>
 * <url>lifxmod.com</url>
 * <credits></credits>
 * <description>Life Is Feudal Crown messages and kill feed System - to give players more immersed gameplay on RP Servers</description>
 */

if (!isObject(LiFxCrownMessages)) {
    new ScriptObject(LiFxCrownMessages);
}

package LiFxCrownMessages {
    function LiFxCrownMessages::setup() {
        LiFxCrownMessages::onHourTick();
        // Ensure to define a separate package for stats if needed.
        LiFx::registerCallback($LiFx::hooks::onJHEndCallbacks, fetchKillDeathStats, LiFxCrownMessages);
    }

    function LiFxCrownMessages::version() {
        return "1.0.0";
    }

    function LiFxCrownMessages::MessageAllWithCustomText(%this, %resultSet) {
        if (%resultSet.ok() && %resultSet.nextRecord()) {
            %message = %resultSet.getFieldValue("Message");
            if (%message !$= "") {
                LiFxUtility::messageAll(2476, %message); // Send custom message to all players
            }
        }
        dbi.remove(%resultSet); // Remove result set after processing
    }

    function LiFxCrownMessages::fetchKillDeathStats(%this) {
        %query = "SELECT c.Name, c.Lastname, COUNT(CASE WHEN d.KillerID = c.ID THEN 1 END) AS kills, COUNT(CASE WHEN d.CharID = c.ID THEN 1 END) AS deaths, " @
                  "CASE WHEN COUNT(CASE WHEN d.CharID = c.ID THEN 1 END) = 0 " @
                  "THEN COUNT(CASE WHEN d.KillerID = c.ID THEN 1 END) " @
                  "ELSE ROUND(COUNT(CASE WHEN d.KillerID = c.ID THEN 1 END) / COUNT(CASE WHEN d.CharID = c.ID THEN 1 END), 2) " @
                  "END AS kd_ratio " @
                  "FROM chars_deathlog d " @
                  "JOIN `character` c ON d.KillerID = c.ID " @
                  "WHERE d.KillerID <> 4294967294 " @
                  "GROUP BY c.ID, c.Name, c.Lastname " @
                  "ORDER BY kills DESC " @
                  "LIMIT 5;";
        dbi.Select(LiFxCrownMessages, "DisplayKillDeathStats", %query);
    }

    function LiFxCrownMessages::DisplayKillDeathStats(%this, %resultSet) {
        if (%resultSet.ok()) {
            // Initialize an empty string to hold the full message
            %fullMessage = "";

            while (%resultSet.nextRecord()) {
                %name = %resultSet.getFieldValue("Name") SPC %resultSet.getFieldValue("Lastname");
                %kills = %resultSet.getFieldValue("kills");
                %deaths = %resultSet.getFieldValue("deaths");
                %kd_ratio = %resultSet.getFieldValue("kd_ratio");

                // Append each record to the full message, adding a new line character
                %fullMessage = %fullMessage @ %name @ " - Kills: " @ %kills @ ", Deaths: " @ %deaths @ ", K/D: " @ %kd_ratio @ "\n";
            }

            // Send the full message to all players
            LiFxUtility::messageAll(2476, %fullMessage);
        }
        dbi.remove(%resultSet); // Ensure result set is removed after processing
    }


    function LiFxCrownMessages::onHourTick() {
        // Define the text messages
        %textArray[0] = "I welcome valhalla";
        %textArray[1] = "Message from A MAD MAN,\n We are mad men now and need to fertilise the women of our ENEMIES!!";
        %textArray[2] = "The bishops have said to Rollo,\n who is unwilling to kiss the king's foot: Whoever receives such a gift, ought to kiss the king's foot\n";
        %textArray[3] = "Quote from Volsunga Saga \n\n When men encounter enemies in the fight, a robust heart is better than a sharp sword.";
        %textArray[4] = "The foolish man thinks he will live forever if he keeps away from fighting;\n but old age wont grant him a truce, even if the spears do.";
        %textArray[5] = "Random Quote from the Crown,\n\n Smart people learn from everything and everyone,\n average people from their experiences,\nstupid people already have all the answers.";
        %textArray[6] = "King Announcement,\n\n if you are being spawn killed, don't spawn.\nInsanity is inflexibly, doing the same thing over and over while hoping for different results.";
        %textArray[7] = "Message from the crown,\n\n Oftentimes it is not numbers that wins the victory, \n but those who fare forward with the most vigor.";
        %textArray[8] = "No Viking believed he could change his destiny,\n ordained as it was by the Norns who wove the fates of gods and men alike but,\n for all that, the way in which he lived his life was up to him.";
        %textArray[9] = "Message from Kevin Crossley-Holland,\n\n Honor and daring, valor, strength and agility,\n all these qualities are prized.";
        %textArray[10] = "Message from His Majesty of the Crown,\n\n Travel far and wide and you should possess the secrets of man,\n Hide in your home to miss the chance on riches.";
        %textArray[11] = "All a people need in order to rise up against tyranny is a leader bold enough to take up the banner.";
        %textArray[12] = "Fear not death for the hour of your doom is set and none may escape it.";
        %textArray[13] = "The path of the Viking is not an easy one, but it is the path we have chosen.";
        %textArray[14] = "I am not afraid of an army of lions led by a sheep; I am afraid of an army of sheep led by a lion.";
        %textArray[15] = "Message from the crown,\n\nI am not afraid of an army of lions led by a sheep;\n I am afraid of an army of sheep led by a lion.";

        %randomIndex = getRandom(0, 15);
        %randomText = %textArray[%randomIndex];

        // Display the random text
        echo(%randomText);
        LiFxUtility::messageAll(2476, %randomText);

        // Schedule the next tick
        %this.Tick = LiFxCrownMessages.schedule(7200000, "onHourTick");
    }
};

// Activate the package to enable the functionality
activatePackage(LiFxCrownMessages);
