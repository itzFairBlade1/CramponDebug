using Rewired;
using UnityEngine;


namespace CramponDebug.src
{
    internal class ConsoleOutput
    {
        // Classes
        private Core core;
        private Tracker tracker;
        private Cache cache;
        private Cfg config;

        // Hastag Property Checks
        private bool lastStateGroundedBool;
        private bool lastStateClimbingBool;

        private bool lastStateUsingAxeL = false;
        private bool lastStateUsingAxeR = false;

        // Checking for keybinds
        private Player player;
        private int playerID;

        // Timers
        // Also used for iceAxes
        private float handTimerStartL = 0f;
        private float handTimerStartR = 0f;

        private float cramponTimerStart = 0f;

        // Checks to not start timer every update
        private bool isTrackingHandTimerL = false;
        private bool isTrackingHandTimerR = false;

        private bool isTrackingCramponTimer = false;


        internal ConsoleOutput(Core core, Tracker tracker, Cache cache, Cfg config)
        {
            this.core = core;
            this.tracker = tracker;
            this.cache = cache;
            this.config = config;
        }

        public void LogTimers()
        {
            player = ReInput.players.GetPlayer(playerID);

            if (cache.IsComplete() && !config.OnlyShowWrongTiming.Value)
                CheckHastagProperties();

            // Check if falling
            if (cache.IsComplete() && isAirborne() && !isFalling())
            {
                if (usingIceAxes())
                {
                    // Left Pick Axe
                    Timer(
                        tracker.iceAxeTimerL,
                        player.GetButtonDown("Arm Left"),
                        cache.iceAxes.usingIceAxeL,
                        ref handTimerStartL,
                        ref isTrackingHandTimerL,
                        "!LEFT AXE input attempted while on cooldown!",
                        "-Left axe was off cooldown for "
                    );

                    // Right Pick Axe
                    Timer(
                        tracker.iceAxeTimerR,
                        player.GetButtonDown("Arm Right"),
                        cache.iceAxes.usingIceAxeR,
                        ref handTimerStartR,
                        ref isTrackingHandTimerR,
                        "!RIGHT AXE input attempted while on cooldown!",
                        "-Right axe was off cooldown for "
                    );
                }

                else
                {
                    // Left Arm
                    Timer(
                        tracker.armTimerL,
                        player.GetButtonDown("Arm Left"),
                        true,
                        ref handTimerStartL,
                        ref isTrackingHandTimerL,
                        "!LEFT ARM input attempted while on cooldown!",
                        "-Left arm was off cooldown for "
                    );

                    // Right Arm
                    Timer(
                        tracker.armTimerR,
                        player.GetButtonDown("Arm Right"),
                        true,
                        ref handTimerStartR,
                        ref isTrackingHandTimerR,
                        "!RIGHT ARM input attempted while on cooldown!",
                        "-Right arm was off cooldown for "
                    );
                }


                // Crampon Timer
                Timer(
                    tracker.cramponTimer,
                    player.GetButtonDown("Wall Kick"),
                    true,
                    ref cramponTimerStart,
                    ref isTrackingCramponTimer,
                    "!CAMPON input attempted while on cooldown!",
                    "-Crampon was off cooldown for "
                );
            }

            // isnt on a hold so we need to stop the timers
            else
            {
                if (isTrackingCramponTimer)
                    isTrackingCramponTimer = false;

                if (isTrackingHandTimerL)
                    isTrackingHandTimerL = false;

                if (isTrackingHandTimerR)
                    isTrackingHandTimerR = false;
            }
        }

        private void Timer(float forceCooldown, bool getButtonDown, bool usingItem, ref float globalTimer, ref bool isTracking, string errorMessage, string succesfulMessage)
        {
            if (usingItem)
            {
                // used Item while on cooldown so log
                if (forceCooldown > 0f && getButtonDown)
                {
                    core.Log(errorMessage, true);
                }

                // start tracking when force cooldown is below 0f
                if (!isTracking && forceCooldown <= 0f)
                {
                    // item just became usable — start the timer
                    globalTimer = Time.time;
                    isTracking = true;
                }

                // stop tracking when force cooldown is above 0f
                else if (isTracking && forceCooldown > 0f)
                {
                    // item was just used - stop the timer
                    float delay = Time.time - globalTimer;
                    if (!config.OnlyShowWrongTiming.Value)
                        core.Log(succesfulMessage + $" {delay:F2} seconds.");
                    isTracking = false;
                }
            }   
        }

        private void CheckHastagProperties()
        {
            // Grounded Check
            bool isGrounded = cache.playerMove.IsGrounded();
            if (!lastStateGroundedBool && isGrounded) core.Log("------------#The Player is grounded#------------");
            lastStateGroundedBool = isGrounded;

            // Climbing Check
            if (!lastStateClimbingBool && isClimbing()) core.Log("------------#The Player has reached a hold#------------");
            lastStateClimbingBool = isClimbing();

            // IceAxe switch Check
            bool usingLeftAxe = cache.iceAxes.usingIceAxeL;
            bool usingRightAxe = cache.iceAxes.usingIceAxeR;

            if (!lastStateUsingAxeL && usingLeftAxe) core.Log("#The Player has equipped the left ice axe#");
            if (lastStateUsingAxeL && !usingLeftAxe) core.Log("#The Player has unequipped the left ice axe#");

            if (!lastStateUsingAxeR && usingRightAxe) core.Log("#The Player has equipped the right ice axe#");
            if (lastStateUsingAxeR && !usingRightAxe) core.Log("#The Player has unequipped the right ice axe#");

            lastStateUsingAxeL = usingLeftAxe;
            lastStateUsingAxeR = usingRightAxe;
        }

        private bool usingIceAxes()
        {
            return (cache.iceAxes.usingIceAxeR || cache.iceAxes.usingIceAxeL);
        }

        private bool isFalling()
        {
            return (cache.climbing.playerBody.velocity.y < -7);
        }

        private bool isAirborne()
        {
            return (!isClimbing() && !cache.playerMove.IsGrounded());
        }

        private bool isClimbing()
        {
            return (cache.climbing.grabbingLeft || cache.climbing.grabbingRight);
        }
    }
}
