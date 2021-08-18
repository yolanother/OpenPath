Equipment can be bought to upgrade ships. It is manufactured at populated planets.

# Developer Details

This section includes notes that will be useful to developers of the game.

## Create a New Equipment Type

An equipment type defines which kind of controller will manage equipment types.

    1. If you expect to have a number of different models of this equipment type that vary more than a simple set of parameters you will need to create a new Abstract Scriptable Object class in Scripts/Data/Config that extends AbstractShipEquipment. If you are unsure then skip this step, you can always refactor later. For an example of this kind of inheritence, see AbstractShipWeapon which defines the base class of all ship weapons.
    2. Create a new Model of the equipment type by creating a new Scriptable Object class for the model. This will extend the Abstract class created in 1 (or AbstractShipEquipment if none created). For example, see ShipToShipLaser which defines a specific model of AbstractShipWeapon
    3. Create a new Controller in Scrips/Controllers to manage the equipment and its use. This will extend AbstractActionController. For example, see ShipWeaponController.
    4. Add the controller you created to any ship that will use this equipment type
    5. Create a debugging toolkit in Scripts/DebuggingEssentials that will allow you to quickly test combinations of features. This needs to extend the class AbstractDebugCommands and takes a generic type that is set to the controller you built in step 3. For example, see ShipWeaponCommands.
    6. Add the debugging toolkit to the Debugging Commands object in a test scene
    7. You will probably want this equipment to be available within the solar systems you inhabit so go to the Solar System Config and add the equipment config (step 2) to it
    8. Regenerate the Solar System in the test scene
    9. Hit play and run your tests