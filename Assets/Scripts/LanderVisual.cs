using UnityEngine;
using System;
using Unity.VisualScripting;

public class LanderVisual : MonoBehaviour
{
   [SerializeField] private ParticleSystem leftThrustParticleSystem;
   [SerializeField] private ParticleSystem rightThrustParticleSystem;
   [SerializeField] private ParticleSystem upThrustParticleSystem;
   [SerializeField] private GameObject landerExplosion;
   [SerializeField] private GameObject landerAttacherPrefab;
   [SerializeField] private Transform landerAttachAnchorPoint;

   private Lander landerObject = null;

   // Start is called before the first frame update
   private void Start()
   {
      landerObject = GetComponent<Lander>();

      landerObject.OnUpForce += OnUpForceInvoke;
      landerObject.OnLeftForce += OnLeftForceInvoke;
      landerObject.OnRightForce += OnRightForceInvoke;
      landerObject.OnBeforeForce += OnBeforeForceInvoke;
      landerObject.OnLanded += OnLandedInvoke;
      // Key OnKeyPicked
      KeyPickUp.OnKeyPicked += KeyPickDrop_OnKeyPicked;

      SetEnableParticleSystem(leftThrustParticleSystem, false);
      SetEnableParticleSystem(rightThrustParticleSystem, false);
      SetEnableParticleSystem(upThrustParticleSystem, false);
   }

   private void KeyPickDrop_OnKeyPicked(object sender, EventArgs e)
   {
      // On Key Picked Show Attacher
      if (landerObject == null)
         return;

      landerObject.landerAttacher = Instantiate(landerAttacherPrefab, landerAttachAnchorPoint.position, Quaternion.identity);
      landerObject.landerAttacher.transform.SetParent(landerObject.transform);
      FixedJoint2D joint2D = landerObject.AddComponent<FixedJoint2D>();
      joint2D.connectedBody = landerObject.landerAttacher.GetComponent<Rigidbody2D>();
      joint2D.autoConfigureConnectedAnchor = false;
   }

   private void OnLandedInvoke(object sender, Lander.LandedEventArgs e)
   {
      // On Crashed
      if (e.landingType != Lander.LANDING_TYPE.SUCCESSFUL_LANDING)
      {
         Instantiate(landerExplosion, transform.position, Quaternion.identity);
         gameObject.SetActive(false);
      }

   }

   // Listeners
   private void OnBeforeForceInvoke(object sender, EventArgs e)
   {
      SetEnableParticleSystem(leftThrustParticleSystem, false);
      SetEnableParticleSystem(upThrustParticleSystem, false);
      SetEnableParticleSystem(rightThrustParticleSystem, false);

   }
   private void OnUpForceInvoke(object sender, EventArgs e)
   {
      SetEnableParticleSystem(leftThrustParticleSystem, true);
      SetEnableParticleSystem(upThrustParticleSystem, true);
      SetEnableParticleSystem(rightThrustParticleSystem, true);

   }

   private void OnLeftForceInvoke(object sender, EventArgs e)
   {
      SetEnableParticleSystem(leftThrustParticleSystem, true);

   }

   private void OnRightForceInvoke(object sender, EventArgs e)
   {
      SetEnableParticleSystem(rightThrustParticleSystem, true);

   }

   private void SetEnableParticleSystem(ParticleSystem particleSystem, bool enabledState)
   {
      ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
      emissionModule.enabled = enabledState;
   }

}
