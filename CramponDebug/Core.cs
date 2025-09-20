using MelonLoader;
using Rewired;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

[assembly: MelonInfo(typeof(CramponDebug.Core), "CramponDebug", "1.0.0", "Fair", null)]
[assembly: MelonGame("TraipseWare", "Peaks of Yore")]

namespace CramponDebug
{
    public class Core : MelonMod
    {
        private bool lastStateGroundedBool;
        private bool lastStateClimbingBool;
        

        private PlayerMove playerMove;
        private Climbing climbing;
        private StemFoot stemFoot;
        private FieldInfo isOnCooldownField;
        private Player player;
        private IceAxe iceaxes;


        private bool isTrackingHandTimerL = false;
        private bool isTrackingHandTimerR = false;

        private bool isTrackingCramponTimer = false;

        private bool lastStateUsingAxeL = false;
        private bool lastStateUsingAxeR = false;


        private float handTimerStartL = 0f;
        private float handTimerStartR = 0f;

        private float cramponTimerStart = 0f;


        private int playerID;




        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("CramponDebug Initialized.");
            isOnCooldownField = typeof(StemFoot).GetField("isOnCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
        }


        public override void OnUpdate()
        {
            GetObjects(); // Loads Scripts
            CheckHastagProperties(); // Logs Hashtag Messages
            CheckTiming(); // Responsible for everything else .-.
        }

        public void CheckTiming()
        {
            // check if scripts loaded
            if (playerMove != null && climbing != null)
            {
                bool isClimbing = climbing.grabbingLeft || climbing.grabbingRight;
                bool isGrounded = playerMove.IsGrounded();

                // checking if player is in air and not falling
                if (!isClimbing && !isGrounded && climbing.playerBody.velocity.y > -7)
                {
                    //checking iceaxes
                    if (iceaxes != null)
                    {
                        if (iceaxes.usingIceAxeR || iceaxes.usingIceAxeL) IceAxeTimer();
                        else HandTimer();
                    }

                    else HandTimer();

                    CramponTimer();
                }

                else
                {
                    isTrackingCramponTimer = false;
                    isTrackingHandTimerL = false;
                    isTrackingHandTimerR = false;
                }
            }
        }


        public void CheckHastagProperties()
        {
            // player touching ground
            if (playerMove != null)
            {
                bool isGrounded = playerMove.IsGrounded();
                if (!lastStateGroundedBool && isGrounded) LoggerInstance.Msg("#The Player is grounded.");
                lastStateGroundedBool = isGrounded;
            }

            // reaching a hold
            if (climbing != null)
            {
                bool isClimbing = climbing.grabbingLeft || climbing.grabbingRight;
                if (!lastStateClimbingBool && isClimbing) LoggerInstance.Msg("#The Player has reached a hold.");
                lastStateClimbingBool = isClimbing;
            }

            // iceaxe switching
            if (iceaxes != null)
            {
                bool usingLextAxe = iceaxes.usingIceAxeL;
                bool usingRightAxe = iceaxes.usingIceAxeR;


                if (!lastStateUsingAxeL && usingLextAxe) LoggerInstance.Msg("#The Player has equipped the left ice axe.");
                if (lastStateUsingAxeL && !usingLextAxe) LoggerInstance.Msg("#The Player has unequipped the left ice axe.");

                if (!lastStateUsingAxeR && usingRightAxe) LoggerInstance.Msg("#The Player has equipped the right ice axe.");
                if (lastStateUsingAxeR && !usingRightAxe) LoggerInstance.Msg("#The Player has unequipped the right ice axe.");


                lastStateUsingAxeL = usingLextAxe;
                lastStateUsingAxeR = usingRightAxe;
            }
        }




        public void GetObjects()
        {
            // Attempts to find scripts
            if (playerMove == null) playerMove = UnityEngine.Object.FindObjectOfType<PlayerMove>();
            if (climbing == null) climbing = UnityEngine.Object.FindObjectOfType<Climbing>();
            if (stemFoot == null) stemFoot = UnityEngine.Object.FindObjectOfType<StemFoot>();
            if (iceaxes == null) iceaxes = UnityEngine.Object.FindObjectOfType<IceAxe>();
            
            // Get Input
            player = ReInput.players.GetPlayer(playerID);

        }


        public void IceAxeTimer()
        {
            float iceAxeForceL = iceaxes.forceCooldownL;
            float iceAxeForceR = iceaxes.forceCooldownR;
            bool usedAxeL = player.GetButtonDown("Arm Left");
            bool usedAxeR = player.GetButtonDown("Arm Right");

            if (iceaxes.usingIceAxeL)
            {
                if (iceAxeForceL > 0f && usedAxeL)
                {
                    LoggerInstance.Warning("!LEFT AXE input attempted while on cooldown!");
                }

                if (!isTrackingHandTimerL && iceAxeForceL <= 0f)
                {
                    // Arm just became usable — start the timer
                    handTimerStartL = Time.time;
                    isTrackingHandTimerL = true;
                }

                else if (isTrackingHandTimerL && iceAxeForceL > 0f)
                {
                    // Arm was just used again — stop the timer
                    float delay = Time.time - handTimerStartL;
                    LoggerInstance.Msg($"-Left axe was off cooldown for {delay:F2} seconds before being used again.");
                    isTrackingHandTimerL = false;
                }
            }

            if (iceaxes.usingIceAxeR)
            {
                if (iceAxeForceR > 0f && usedAxeR)
                {
                    LoggerInstance.Warning("!RIGHT AXE input attempted while on cooldown!");
                }

                if (!isTrackingHandTimerR && iceAxeForceR <= 0f)
                {
                    // Arm just became usable — start the timer
                    handTimerStartR = Time.time;
                    isTrackingHandTimerR = true;
                }

                else if (isTrackingHandTimerR && iceAxeForceR > 0f)
                {
                    // Arm was just used again — stop the timer
                    float delay = Time.time - handTimerStartR;
                    LoggerInstance.Msg($"-Right axe was off cooldown for {delay:F2} seconds before being used again.");
                    isTrackingHandTimerR = false;
                }
            }
        }

        public void HandTimer()
        {
            float armForceL = climbing.armForceTimerL;
            float armForceR = climbing.armForceTimerR;
            bool usedArmL = player.GetButtonDown("Arm Left");
            bool usedArmR = player.GetButtonDown("Arm Right");

            if (armForceL > 0f && usedArmL)
            {
                LoggerInstance.Warning("!LEFT ARM input attempted while on cooldown!");
            }

            if (!isTrackingHandTimerL && armForceL <= 0f)
            {
                // Arm just became usable — start the timer
                handTimerStartL = Time.time;
                isTrackingHandTimerL = true;
            }
            
            else if (isTrackingHandTimerL && armForceL > 0f)
            {
                // Arm was just used again — stop the timer
                float delay = Time.time - handTimerStartL;
                LoggerInstance.Msg($"-Left hand was off cooldown for {delay:F2} seconds before being used again.");
                isTrackingHandTimerL = false;
            }



            if (armForceR > 0f && usedArmR)
            {
                LoggerInstance.Warning("!RIGHT ARM input attempted while on cooldown!");
            }

            if (!isTrackingHandTimerR && armForceR <= 0f)
            {
                // Arm just became usable — start the timer
                handTimerStartR = Time.time;
                isTrackingHandTimerR = true;
            }
            
            else if (isTrackingHandTimerR && armForceR > 0f)
            {
                // Arm was just used again — stop the timer
                float delay = Time.time - handTimerStartR;
                LoggerInstance.Msg($"-Right hand was off cooldown for {delay:F2} seconds before being used again.");
                isTrackingHandTimerR = false;
            }

        }




        public void CramponTimer()
        {
            if (stemFoot != null && isOnCooldownField != null)
            {
                bool isOnCooldown = (bool)isOnCooldownField.GetValue(stemFoot);
                bool usedWallKick = player.GetButtonDown("Wall Kick");

                // Player tried to Wall Kick during cooldown
                if (isOnCooldown && usedWallKick)
                {
                    LoggerInstance.Warning("!CRAMPON input attempted while on cooldown!");
                }

                // Crampons came off cooldown - Start timing
                if (!isOnCooldown && !isTrackingCramponTimer)
                {
                    cramponTimerStart = Time.time;
                    isTrackingCramponTimer = true;
                }

                // Crampons went on cooldown again - Log timing and missed inputs
                else if (isOnCooldown && isTrackingCramponTimer)
                {
                    float delay = Time.time - cramponTimerStart;
                    LoggerInstance.Msg($"*Crampons were off cooldown for {delay:F2} seconds before being used.");
                    isTrackingCramponTimer = false;
                }
            }
        }

    }
}
